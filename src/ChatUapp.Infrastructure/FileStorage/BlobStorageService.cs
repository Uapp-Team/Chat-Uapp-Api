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

/// <summary>
/// Service for managing file storage operations in Azure Blob Storage.
/// Supports file uploads, retrieval, deletion, and SAS URL generation.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly IBlobContainer _blobContainer;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly string _connectionString;
    private readonly string _containerPath;

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
        _blobServiceClient = new BlobServiceClient(_connectionString);
    }

    /// <summary>
    /// Saves a file (Base64 string) to the default container.
    /// Overwrites if the file already exists.
    /// </summary>
    public async Task<string> SaveAsync(string base64, string fileName)
    {
        using var stream = ConvertBase64ToStream(base64);
        await _blobContainer.SaveAsync(fileName, stream, overrideExisting: true);
        return fileName;
    }

    /// <summary>
    /// Reads a file from the default container and returns it as a Base64 string.
    /// </summary>
    public async Task<string> ReadAsync(string fileName)
    {
        EnsureFileNameNotEmpty(fileName);

        if (!await _blobContainer.ExistsAsync(fileName))
            throw new FileNotFoundException($"File '{fileName}' not found in blob container.");

        using var stream = await _blobContainer.GetAsync(fileName);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    /// <summary>
    /// Deletes a file from the default container if it exists.
    /// </summary>
    public async Task DeleteAsync(string fileName)
    {
        EnsureFileNameNotEmpty(fileName);

        if (await _blobContainer.ExistsAsync(fileName))
        {
            await _blobContainer.DeleteAsync(fileName);
        }
    }

    /// <summary>
    /// Checks if a file exists in the default container.
    /// </summary>
    public Task<bool> ExistsAsync(string fileName)
    {
        EnsureFileNameNotEmpty(fileName);
        return _blobContainer.ExistsAsync(fileName);
    }

    /// <summary>
    /// Saves an image to Azure Blob Storage, replacing if exists.
    /// If base64 stream is null/empty, keeps the old file name.
    /// </summary>
    /// <param name="fileStream">Base64 file content (nullable).</param>
    /// <param name="newFileName">New file name (nullable).</param>
    /// <param name="oldFileName">Old file name to keep if no new file provided.</param>
    /// <returns>Final file name that should be stored in DB.</returns>
    public async Task<string> SaveImagesAsync(string? fileStream, string? newFileName, string? oldFileName = null)
    {
        if (string.IsNullOrWhiteSpace(fileStream))
            return oldFileName ?? string.Empty;

        if (string.IsNullOrWhiteSpace(newFileName))
            return oldFileName ?? string.Empty;

        var extension = Path.GetExtension(newFileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Invalid image type: {extension}");

        var context = await GetUserContainerAsync(newFileName);
        var blobClient = context.ContainerClient.GetBlobClient(context.BlobPath);

        //  Check if file already exists
        if (await blobClient.ExistsAsync())
        {
            // File exists → just return relative path without uploading
            var parts = context.BlobPath.Split('/');
            return parts.Length >= 3
                ? string.Join('/', parts[^3..])
                : context.BlobPath;
        }

        // File does not exist → upload it
        using var stream = ConvertBase64ToStream(fileStream);
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = GetMimeType(newFileName)
            }
        };

        await blobClient.UploadAsync(stream, uploadOptions);

        var finalParts = context.BlobPath.Split('/');
        return finalParts.Length >= 3
            ? string.Join('/', finalParts[^3..])
            : context.BlobPath;
    }




    /// <summary>
    /// Generates a SAS URL for accessing a file.
    /// </summary>
    public Task<string> GetUrlAsync(string fileName, int expireInDays = 365)
    {
        EnsureFileNameNotEmpty(fileName);

        var (containerName, blobName) = GetContainerAndBlobName(fileName);

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!containerClient.CanGenerateSasUri)
            throw new AppValidationException("SAS generation is not possible with the provided credentials.");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1),
            ExpiresOn = DateTimeOffset.UtcNow.AddDays(expireInDays)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return Task.FromResult(blobClient.GenerateSasUri(sasBuilder).ToString());
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
            throw new AppValidationException($"Invalid Base64 string. Error: {ex.Message}");
        }
    }

    private async Task<BlobUploadContext> GetUserContainerAsync(string fileName)
    {
        var tenantId = GetCurrentTenantId();

        var fileCategory = FileTypeClassifier.GetFileCategory(fileName);

        // Use the project container name as container name
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerPath);

        // Create container if not exists
        await containerClient.CreateIfNotExistsAsync();

        var blobPath = BlobPathBuilder.BuildPath(tenantId, fileCategory, fileName);
        var context = new BlobUploadContext(containerClient, blobPath);

        if (!context.IsValid)
            throw new AppValidationException("Invalid blob upload context.");

        return context;
    }

    private string GetCurrentUserId(string fallbackUserId = "anonymous") =>
        _currentUser?.Id?.ToString() ?? fallbackUserId;

    private string GetCurrentTenantId(string fallbackTenantId = "Default") =>
        _currentTenant?.Id?.ToString() ?? fallbackTenantId;

    private string GetMimeType(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }

    private (string containerName, string blobName) GetContainerAndBlobName(string fileName)
    {
        string containerName = _containerPath;

        // If fileName is already a full path from Azure, use it directly
        if (fileName.StartsWith("tenant/") || fileName.Contains("/others/"))
        {
            return (containerName, fileName);
        }

        // Otherwise reconstruct using BuildPath
        var tenantId = GetCurrentTenantId();
        var fileCategory = FileTypeClassifier.GetFileCategory(fileName);

        // fileName here is assumed to be just the original file name, not the full blob path
        string blobName = BlobPathBuilder.UpdateBuildPath(tenantId, fileCategory, fileName);


        return (containerName, blobName);
    }


    private static void EnsureFileNameNotEmpty(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new AppValidationException("File name cannot be null or empty.");
    }
}