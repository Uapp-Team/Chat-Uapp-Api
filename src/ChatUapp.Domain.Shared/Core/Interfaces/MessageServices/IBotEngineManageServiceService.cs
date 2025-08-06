using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces.MessageServices;

public interface IBotEngineManageServiceService
{
    Task<string> AskAnything(string message);
}
