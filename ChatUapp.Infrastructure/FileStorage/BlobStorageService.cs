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

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="currentUser">The currently authenticated user.</param>
        /// <exception cref="AppValidationException">Thrown if the Azure Blob Storage connection string is missing.</exception>
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



        /// <summary>
        /// Uploads a file to the user's blob container.
        /// </summary>
        /// <param name="fileStream">The file stream to upload.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="AppValidationException">Thrown if a file with the same name already exists.</exception>
        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var context = await GetUserContainerAsync(fileName);
            var blobClient = context?.ContainerClient?.GetBlobClient(context.BlobPath);

            if (await blobClient.ExistsAsync())
                throw new AppValidationException("A file with the same name already exists.");
            // Detect MIME type
            var fileCategory = FileTypeClassifier.GetFileCategory(fileName);
            var contentType = GetMimeType(fileName);

            var uploadOptions = new BlobUploadOptions();

            if (fileCategory == "images")
            {
                uploadOptions.HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                };
            }
            await blobClient.UploadAsync(fileStream, uploadOptions);

            return context.BlobPath; // secure temporary URL
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

        /// <summary>
        /// Generates a temporary public URL (SAS URI) for accessing a file in the user's container.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="expireInMinutes">The expiration time in minutes. Default is 3 minutes.</param>
        /// <returns>The temporary access URL as a string.</returns>
        /// <exception cref="AppValidationException">Thrown if the file does not exist.</exception>
        public async Task<string> GetTemporaryUrlAsync(string ? blobPath , int expireInMinutes = 30)
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

            // Check if the blob exists
            if (!await blobClient.ExistsAsync())
            {
                throw new AppValidationException("File not found in blob storage.");
            }

            // ✅ Correct SAS builder
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1), // ✅ start buffer
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expireInMinutes)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read); // ✅ set permissions

            // ✅ Generate URI
            var sasUri = blobClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString();
        }

        /// <summary>
        /// Deletes a file from the user's blob container if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string fileName)
        {
            var context = await GetUserContainerAsync(fileName);
            var blobClient = context?.ContainerClient?.GetBlobClient(context.BlobPath);
            await blobClient.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Checks if a file exists in the user's blob container.
        /// </summary>
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public async Task<bool> ExistsAsync(string fileName)
        {
            var context = await GetUserContainerAsync(fileName);
            var blobClient = context?.ContainerClient?.GetBlobClient(context.BlobPath);
            return await blobClient.ExistsAsync();
        }

        public async Task<Stream> ConvertBase64ToStream(string base64String)
        {
            // Remove data URI prefix if present
            if (base64String.Contains(","))
            {
                base64String = base64String.Substring(base64String.IndexOf(",") + 1);
            }

            byte[] bytes = Convert.FromBase64String(base64String);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Retrieves the ID of the currently authenticated user.
        /// </summary>
        /// <returns>The user's ID as a string.</returns>
        /// <exception cref="AppValidationException">Thrown if the user is not authenticated.</exception>
        private string GetCurrentUserId(string fallbackUserId = null)
        {
            var userId = _currentUser?.Id?.ToString();

            return !string.IsNullOrWhiteSpace(userId)
                ? userId
                : !string.IsNullOrWhiteSpace(fallbackUserId)
                    ? fallbackUserId
                    : "anonymous";
        }

        private string GetCurrentTenantId(string fallbackTenantId = null)
        {
            var tenantId = _currentTenant?.Id?.ToString();

            return !string.IsNullOrWhiteSpace(tenantId)
                ? tenantId
                : !string.IsNullOrWhiteSpace(fallbackTenantId)
                    ? fallbackTenantId
                    : "Default_Tenant";
        }


        /// <summary>
        /// Gets or creates a blob container specific to the current user.
        /// </summary>
        /// <returns>The <see cref="BlobContainerClient"/> for the user's container.</returns>
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

            return new BlobUploadContext
            {
                ContainerClient = containerClient,
                BlobPath = blobPath
            };
        }


    }
}
