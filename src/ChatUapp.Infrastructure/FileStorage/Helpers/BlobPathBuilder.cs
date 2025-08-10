namespace ChatUapp.Infrastructure.FileStorage.Helpers;

public static class BlobPathBuilder
{
    public static string BuildPath(string tenantId, string fileType, string originalFileName)
    {
        fileType = string.IsNullOrWhiteSpace(fileType) ? "others" : fileType.ToLowerInvariant();
        var date = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(tenantId))
            return $"{fileType}/{date:yyyy}/{date:MM}/{originalFileName}";

        return $"tenant/{tenantId}/{fileType}/{date:yyyy}/{date:MM}/{originalFileName}";
    }

    public static string UpdateBuildPath(string tenantId, string fileType, string originalFileName)
    {
        fileType = string.IsNullOrWhiteSpace(fileType) ? "others" : fileType.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(tenantId))
            return $"{fileType}/{originalFileName}";

        return $"tenant/{tenantId}/{fileType}/{originalFileName}";
    }
}
