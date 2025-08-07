using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.FileStorage;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;

namespace ChatUapp.Infrastructure.FileStorage
{
    /// <summary>
    /// Provides functionality to upload, retrieve, delete, and verify user profile images in Azure Blob Storage.
    /// </summary>
    public class BlobStorageService : BlobProviderBase, IBlobStorageService
    {

        private readonly IBlobContainer _blobContainer;
        private readonly IConfiguration _configuration;

        public BlobStorageService(
            IBlobContainer blobContainer,
            IConfiguration configuration
            )
        {
            _blobContainer = blobContainer;
            _configuration = configuration;
        }

        //  CREATE or REPLACE FILE (auto-replaces if exists)
        public async Task<string> SaveAsync(string fileStream, string fileName)
        {
            using var stream = ConvertBase64ToStream(fileStream);
            await _blobContainer.SaveAsync(fileName, stream, overrideExisting: true);
            return fileName;
        }
        // ✅ READ FILE (returns base64 string)
        public async Task<string> ReadAsync(string fileName)
        {
            if (!await _blobContainer.ExistsAsync(fileName))
                throw new FileNotFoundException("File not found in blob container.");

            using var stream = await _blobContainer.GetAsync(fileName);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        // ✅ DELETE FILE
        public async Task DeleteAsync(string fileName)
        {
            if (await _blobContainer.ExistsAsync(fileName))
            {
                await _blobContainer.DeleteAsync(fileName);
            }
        }

        //  CHECK IF FILE EXISTS
        public async Task<bool> ExistsAsync(string fileName)
        {
            return await _blobContainer.ExistsAsync(fileName);
        }

        //  Convert Base64 to Stream
        private Stream ConvertBase64ToStream(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new AppValidationException("Base64 string is null or empty.");

            // Remove data URI scheme if present (e.g., "data:image/png;base64,...")
            var base64Data = base64;
            var commaIndex = base64.IndexOf(',');
            if (commaIndex >= 0)
            {
                base64Data = base64.Substring(commaIndex + 1);
            }

            // Clean whitespace/new lines
            base64Data = base64Data.Trim();

            try
            {
                var bytes = Convert.FromBase64String(base64Data);
                return new MemoryStream(bytes);
            }
            catch (FormatException ex)
            {
                throw new AppValidationException($"Input is not a valid Base64 string : Error {ex.Message}");
            }
        }


        public Task<string> GetUrlAsync(string fileName, int expireInMinutes = 365)
        {
            var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
            var containerPath = _configuration["AzureBlobStorage:ContainerName"];


            Ensure.NotNull(containerPath, nameof(containerPath));

            // Extract container and virtual folder
            var segments = containerPath!.Split('/', 2);
            var containerName = segments[0]; // e.g., "chatuapp"
            var virtualPath = segments.Length > 1 ? segments[1] : null;

            var blobName = string.IsNullOrEmpty(virtualPath)
                ? fileName
                : $"{virtualPath.TrimEnd('/')}/{fileName}";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            // Generate a SAS URL only if the container client supports generating it
            if (!containerClient.CanGenerateSasUri)
            {
                throw new AppValidationException("SAS generation is not possible with the provided credentials.");
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(expireInMinutes)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return Task.FromResult(sasUri.ToString());
        }
    }
}
