using System.IO;
using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces.FileStorage
{
    public interface IBlobStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);
        Task<string> GetUrlAsync(string fileName, int expireInMinutes = 3);
        Task DeleteAsync(string fileName);
        Task<Stream> ConvertBase64ToStream(string base64String);
    }
}
