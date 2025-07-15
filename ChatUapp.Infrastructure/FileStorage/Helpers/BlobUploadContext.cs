using Azure.Storage.Blobs;

namespace ChatUapp.Infrastructure.FileStorage.Helpers
{
    public class BlobUploadContext
    {
        public BlobContainerClient? ContainerClient { get; set; }
        public string? BlobPath { get; set; }
    }
}
