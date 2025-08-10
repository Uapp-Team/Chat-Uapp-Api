using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ChatUapp.Core.Interfaces.FileStorage;
using ChatUapp.Infrastructure.FileStorage.Helpers;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Infrastructure.FileStorage;

public class BlobStorageService : IBlobStorageService
{
    private readonly IBlobContainer _blobContainer;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly string _connectionString;
    private readonly string _containerPath;

    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public BlobStorageService(
        IBlobContainer blobContainer,
        IConfiguration configuration,
        BlobServiceClient blobServiceClient,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant)
    {
        _blobContainer = blobContainer ?? throw new ArgumentNullException(nameof(blobContainer));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _currentTenant = currentTenant ?? throw new ArgumentNullException(nameof(currentTenant));

        _connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new AppValidationException("Azure Blob Storage connection string is not configured.");

        _containerPath = configuration["AzureBlobStorage:ContainerName"]
            ?? throw new AppValidationException("Azure Blob Storage container name is not configured.");

        _blobServiceClient = blobServiceClient ?? new BlobServiceClient(_connectionString);
    }

    public async Task<string> SaveAsync(string base64, string fileName)
    {
        if (string.IsNullOrWhiteSpace(base64) || string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        using var stream = ConvertBase64ToStream(base64);
        await _blobContainer.SaveAsync(fileName, stream, overrideExisting: true);
        return fileName;
    }

    public async Task<string> ReadAsync(string fileName)
    {
        ValidateFileName(fileName);
        if (!await _blobContainer.ExistsAsync(fileName))
            throw new AppValidationException($"File '{fileName}' not found.");

        using var stream = await _blobContainer.GetAsync(fileName);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return Convert.ToBase64String(ms.ToArray());
    }

    public async Task DeleteAsync(string fileName)
    {
        ValidateFileName(fileName);
        if (await _blobContainer.ExistsAsync(fileName))
            await _blobContainer.DeleteAsync(fileName);
    }

    public Task<bool> ExistsAsync(string fileName)
    {
        ValidateFileName(fileName);
        return _blobContainer.ExistsAsync(fileName);
    }

    public async Task<string> SaveImagesAsync(string? base64File, string? newFileName, string? oldFileName = null)
    {
        if (string.IsNullOrWhiteSpace(base64File) || string.IsNullOrWhiteSpace(newFileName))
            return oldFileName ?? string.Empty;

        var ext = Path.GetExtension(newFileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(ext))
            throw new AppValidationException($"Invalid image type: {ext}");

        var context = await GetUploadContextAsync(newFileName);
        var blobClient = context.ContainerClient.GetBlobClient(context.BlobPath);

        if (await blobClient.ExistsAsync())
            return GetRelativeBlobPath(context.BlobPath);

        using var stream = ConvertBase64ToStream(base64File);
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = MimeHelper.GetMimeType(newFileName) }
        };

        await blobClient.UploadAsync(stream, uploadOptions);
        return GetRelativeBlobPath(context.BlobPath);
    }

    public Task<string> GetUrlAsync(string fileName, int expireInDays = 365)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Task.FromResult(string.Empty);

        var (containerName, blobName) = GetContainerAndBlobName(fileName);
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!containerClient.CanGenerateSasUri)
            return Task.FromResult(fileName);

        try
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(expireInDays)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(sasBuilder).ToString();
            return Task.FromResult(sasUri);
        }
        catch
        {
            // If something fails during SAS generation, fallback to returning the fileName
            return Task.FromResult(fileName);
        }
    }

    private Stream ConvertBase64ToStream(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            throw new AppValidationException("Base64 string is null or empty.");

        var commaIndex = base64.IndexOf(',');
        var base64Data = commaIndex >= 0 ? base64[(commaIndex + 1)..] : base64;

        try
        {
            var bytes = Convert.FromBase64String(base64Data.Trim());
            return new MemoryStream(bytes);
        }
        catch (FormatException ex)
        {
            throw new AppValidationException($"Invalid Base64 string: {ex.Message}");
        }
    }

    private async Task<BlobUploadContext> GetUploadContextAsync(string fileName)
    {
        var tenantId = GetCurrentTenantId();
        var fileCategory = FileTypeClassifier.GetFileCategory(fileName);

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerPath);
        await containerClient.CreateIfNotExistsAsync();

        var blobPath = BlobPathBuilder.BuildPath(tenantId, fileCategory, fileName);
        var context = new BlobUploadContext(containerClient, blobPath);

        if (!context.IsValid)
            throw new AppValidationException("Invalid blob upload context.");

        return context;
    }

    private string GetCurrentTenantId(string fallback = "Default") =>
        _currentTenant?.Id?.ToString() ?? fallback;

    private (string containerName, string blobName) GetContainerAndBlobName(string fileName)
    {
        var containerName = _containerPath;

        if (fileName.StartsWith("tenant/") || fileName.Contains("/others/"))
            return (containerName, fileName);

        var tenantId = GetCurrentTenantId();
        var fileCategory = FileTypeClassifier.GetFileCategory(fileName);
        var blobName = BlobPathBuilder.UpdateBuildPath(tenantId, fileCategory, fileName);

        return (containerName, blobName);
    }

    private static string GetRelativeBlobPath(string fullBlobPath)
    {
        var parts = fullBlobPath.Split('/');
        return parts.Length >= 3 ? string.Join('/', parts[^3..]) : fullBlobPath;
    }

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new AppValidationException("File name cannot be null or empty.");
    }
}
