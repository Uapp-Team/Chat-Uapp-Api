using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ChatUapp.Core.Interfaces.FileStorage;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Users;

namespace ChatUapp.Infrastructure.FileStorage
{
    /// <summary>
    /// Provides functionality to upload, retrieve, delete, and verify user profile images in Azure Blob Storage.
    /// </summary>
    public class UserProfileImageUploader : IUserProfileImageUploader
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileImageUploader"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="currentUser">The currently authenticated user.</param>
        /// <exception cref="AppValidationException">Thrown if the Azure Blob Storage connection string is missing.</exception>
        public UserProfileImageUploader(IConfiguration configuration, ICurrentUser currentUser)
        {
            _configuration = configuration;
            _currentUser = currentUser;

            var connectionString = _configuration["AzureBlobStorage:ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new AppValidationException("Azure Blob Storage connection string is not configured.");
            }

            _blobServiceClient = new BlobServiceClient(connectionString);
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
            var container = await GetUserContainerAsync();
            var blobClient = container.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                throw new AppValidationException("A file with the same name already exists.");
            }

            await blobClient.UploadAsync(fileStream, overwrite: false);
            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Generates a temporary public URL (SAS URI) for accessing a file in the user's container.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="expireInMinutes">The expiration time in minutes. Default is 3 minutes.</param>
        /// <returns>The temporary access URL as a string.</returns>
        /// <exception cref="AppValidationException">Thrown if the file does not exist.</exception>
        public async Task<string> GetTemporaryUrlAsync(string fileName, int expireInMinutes = 3)
        {
            var container = await GetUserContainerAsync();
            var blobClient = container.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
            {
                throw new AppValidationException("File not found.");
            }

            var sasUri = blobClient.GenerateSasUri(
                BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddMinutes(expireInMinutes)
            );

            return sasUri.ToString();
        }

        /// <summary>
        /// Deletes a file from the user's blob container if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string fileName)
        {
            var container = await GetUserContainerAsync();
            var blobClient = container.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Checks if a file exists in the user's blob container.
        /// </summary>
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public async Task<bool> ExistsAsync(string fileName)
        {
            var container = await GetUserContainerAsync();
            var blobClient = container.GetBlobClient(fileName);
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
        private string GetCurrentUserId()
        {
            var userId = _currentUser.Id?.ToString();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new AppValidationException("User is not authenticated.");
            }
            return userId;
        }

        /// <summary>
        /// Gets or creates a blob container specific to the current user.
        /// </summary>
        /// <returns>The <see cref="BlobContainerClient"/> for the user's container.</returns>
        private async Task<BlobContainerClient> GetUserContainerAsync()
        {
            var containerName = $"user-{GetCurrentUserId()}".ToLower();
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            return containerClient;
        }
    }
}
