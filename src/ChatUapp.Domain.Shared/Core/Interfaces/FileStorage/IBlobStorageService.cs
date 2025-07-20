using System.IO;
using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces.FileStorage
{
    public interface IBlobStorageService
    {
        Task<string> SaveAsync(string fileStream, string fileName);
        Task<string> GetUrlAsync(string fileName, int expireInMinutes = 3);
        Task DeleteAsync(string fileName);
    }
}
