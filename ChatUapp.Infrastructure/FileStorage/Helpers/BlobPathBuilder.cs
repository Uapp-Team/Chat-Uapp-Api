namespace ChatUapp.Infrastructure.FileStorage.Helpers
{
    public static class BlobPathBuilder
    {
        public static string BuildPath(string tenantId, string userId, string fileType, string originalFileName)
        {
            tenantId ??= "Default_Tenant";
            userId ??= "anonymous";
            fileType = string.IsNullOrWhiteSpace(fileType) ? "others" : fileType.ToLowerInvariant();

            var date = DateTime.UtcNow;
            var uniqueFileName = $"{date:yyyyMMdd}_{Guid.NewGuid():N}_{originalFileName}";

            return $"tenant-{tenantId}/user-{userId}/{fileType}/{date:yyyy}/{date:MM}/{uniqueFileName}";
        }
    }
}
