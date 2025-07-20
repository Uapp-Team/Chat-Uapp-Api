using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ChatUapp.Core.Interfaces.FileStorage;
using ChatUapp.Infrastructure.FileStorage.Helpers;
using Microsoft.Extensions.Configuration;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Infrastructure.FileStorage
{
    /// <summary>
    /// Provides functionality to upload, retrieve, delete, and verify user profile images in Azure Blob Storage.
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;

        
        public BlobStorageService(IConfiguration configuration, ICurrentUser currentUser, ICurrentTenant currentTenant)
        {
            _configuration = configuration;
            _currentUser = currentUser;

            var connectionString = _configuration["AzureBlobStorage:ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new AppValidationException("Azure Blob Storage connection string is not configured.");
            }

            _blobServiceClient = new BlobServiceClient(connectionString);
            _currentTenant = currentTenant;
        }




        public async Task<string> SaveAsync(string fileStream, string fileName)
        {
            var context = await GetUserContainerAsync(fileName);
            if (context == null || context.ContainerClient == null)
            {
                throw new InvalidOperationException("Blob container context could not be resolved.");
            }

            var blobClient = context.ContainerClient.GetBlobClient(context.BlobPath);

            // Detect MIME type
            var fileCategory = FileTypeClassifier.GetFileCategory(fileName);
            var contentType = GetMimeType(fileName);

            var uploadOptions = new BlobUploadOptions();

            if (fileCategory == "images")
            {
                uploadOptions.HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType,
                    
                };
            }
            var Stream = await ConvertBase64ToStream(fileStream);
            // Upload and overwrite existing blob
            var uploadResponse = await blobClient.UploadAsync(Stream, options: uploadOptions, cancellationToken: default);

            // Optionally log or inspect uploadResponse if needed
            if (uploadResponse == null || uploadResponse.GetRawResponse().Status != 201)
            {
                throw new Exception("Blob upload failed.");
            }

            return context.BlobPath;
        }





        public Task<string> GetUrlAsync(string ? blobPath , int expireInMinutes = 30)
        {
            if (string.IsNullOrWhiteSpace(blobPath))
            {
                throw new AppValidationException("Invalid blob path.");
            }

            // Determine file category from path (based on extension)
            var fileCategory = FileTypeClassifier.GetFileCategory(blobPath);
            var containerName = FileTypeClassifier.GetContainerName(fileCategory);

            // Get container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobPath);

           

            //  Correct SAS builder
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1), 
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1000)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read); 

            var sasUri =  blobClient.GenerateSasUri(sasBuilder).ToString();

            return  Task.FromResult(sasUri);
        }


        public async Task DeleteAsync(string fileName)
        {
            var context = await GetUserContainerAsync(fileName);

            if (context == null || context.ContainerClient == null || string.IsNullOrWhiteSpace(context.BlobPath))
            {
                throw new AppValidationException("Invalid blob context. Cannot delete blob.");
            }

            var blobClient = context.ContainerClient.GetBlobClient(context.BlobPath);

            if (blobClient == null)
            {
                throw new AppValidationException("Failed to resolve blob client.");
            }

            await blobClient.DeleteIfExistsAsync();
        }

        private async Task<Stream> ConvertBase64ToStream(string base64String)
        {
            // Remove data URI prefix if present
            if (base64String.Contains(","))
            {
                base64String = base64String.Substring(base64String.IndexOf(",") + 1);
            }

            byte[] bytes = Convert.FromBase64String(base64String);
            return new MemoryStream(bytes);
        }

        private string GetCurrentUserId(string fallbackUserId = "")
        {
            var userId = _currentUser?.Id?.ToString();

            return !string.IsNullOrWhiteSpace(userId)
                ? userId
                : !string.IsNullOrWhiteSpace(fallbackUserId)
                    ? fallbackUserId
                    : "anonymous";
        }

        private string GetCurrentTenantId(string fallbackTenantId = "")
        {
            var tenantId = _currentTenant?.Id?.ToString();

            return !string.IsNullOrWhiteSpace(tenantId)
                ? tenantId
                : !string.IsNullOrWhiteSpace(fallbackTenantId)
                    ? fallbackTenantId
                    : "Default_Tenant";
        }

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

        private async Task<BlobUploadContext> GetUserContainerAsync(string fileName)
        {
            string tenantId = GetCurrentTenantId();
            string userId = GetCurrentUserId();

            string fileCategory = FileTypeClassifier.GetFileCategory(fileName);
            string containerName = FileTypeClassifier.GetContainerName(fileCategory);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Build relative blob path
            string blobPath = BlobPathBuilder.BuildPath(tenantId, userId, fileCategory, fileName);

            var context = new BlobUploadContext(containerClient, blobPath);

            if (!context.IsValid)
            {
                throw new InvalidOperationException("Invalid blob upload context.");
            }
            return context;
        }
    }
}
