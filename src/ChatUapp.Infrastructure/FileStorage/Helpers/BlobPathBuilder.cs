namespace ChatUapp.Infrastructure.FileStorage.Helpers
{
    public static class BlobPathBuilder
    {
        public static string BuildPath( string tenantId, string fileType, string originalFileName)
        {
            fileType = string.IsNullOrWhiteSpace(fileType) ? "others" : fileType.ToLowerInvariant();

            var date = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                // Tenant ID is empty → no "tenant/{tenantId}" segment
                return $"{fileType}/{date:yyyy}/{date:MM}/{originalFileName}";
            }
            else
            {
                // Tenant ID exists → include "tenant/{tenantId}"
                return $"tenant/{tenantId}/{fileType}/{date:yyyy}/{date:MM}/{originalFileName}";
            }
        }
        public static string UpdateBuildPath(string tenantId, string fileType, string originalFileName)
        {
            fileType = string.IsNullOrWhiteSpace(fileType) ? "others" : fileType.ToLowerInvariant();

            var date = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                // Tenant ID is empty → no "tenant/{tenantId}" segment
                return $"{fileType}/{originalFileName}";
            }
            else
            {
                // Tenant ID exists → include "tenant/{tenantId}"
                return $"tenant/{tenantId}/{fileType}/{originalFileName}";
            }
        }
    }
}
