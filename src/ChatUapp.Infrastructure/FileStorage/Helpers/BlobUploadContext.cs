using Azure.Storage.Blobs;

namespace ChatUapp.Infrastructure.FileStorage.Helpers
{
    /// <summary>
    /// Represents the context needed for uploading a blob to Azure Blob Storage.
    /// </summary>
    public class BlobUploadContext
    {
        public BlobUploadContext(BlobContainerClient containerClient, string blobPath)
        {
            ContainerClient = containerClient ?? throw new ArgumentNullException(nameof(containerClient));
            BlobPath = !string.IsNullOrWhiteSpace(blobPath) ? blobPath : throw new ArgumentNullException(nameof(blobPath));
        }

        /// <summary>
        /// The Azure Blob container client where the file will be uploaded.
        /// </summary>
        public BlobContainerClient ContainerClient { get; private set; }

        /// <summary>
        /// The relative path within the blob container where the file will be stored.
        /// </summary>
        public string BlobPath { get; private set; }

        /// <summary>
        /// Indicates whether the context is valid (i.e., both container client and blob path are set).
        /// </summary>
        public bool IsValid => ContainerClient != null && !string.IsNullOrWhiteSpace(BlobPath);
    }
}
