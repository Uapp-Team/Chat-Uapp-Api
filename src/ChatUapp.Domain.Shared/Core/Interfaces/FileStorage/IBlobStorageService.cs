using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces.FileStorage
{
    public interface IBlobStorageService
    {
        Task<string> SaveAsync(string base64, string fileName);
        Task<string> ReadAsync(string fileName);
        Task DeleteAsync(string fileName);
        Task<bool> ExistsAsync(string fileName);
        Task<string> SaveImagesAsync(string? base64File, string? newFileName, string? oldFileName = null);
        Task<string> GetUrlAsync(string fileName, int expireInDays = 365);
    }
}
