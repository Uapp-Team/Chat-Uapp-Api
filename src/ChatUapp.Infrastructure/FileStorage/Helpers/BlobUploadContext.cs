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

        public BlobContainerClient ContainerClient { get; }
        public string BlobPath { get; }
        public bool IsValid => ContainerClient != null && !string.IsNullOrWhiteSpace(BlobPath);
    }
}
