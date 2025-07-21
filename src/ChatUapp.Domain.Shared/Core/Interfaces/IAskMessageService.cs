using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces
{
    public interface IAskMessageService
    {
        Task<string> AskAnything(string message);
    }
}
