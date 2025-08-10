namespace ChatUapp.Infrastructure.FileStorage.Helpers
{
    public static class FileTypeClassifier
    {
        public static string GetFileCategory(string fileName)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();

            return ext switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" => "images",
                ".pdf" => "documents",
                ".txt" => "texts",
                _ => "others"
            };
        }
    }
}
