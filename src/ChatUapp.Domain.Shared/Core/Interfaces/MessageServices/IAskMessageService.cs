using System.Threading.Tasks;

namespace ChatUapp.Core.Interfaces.MessageServices;

public interface IAskMessageService
{
    Task<string> AskAnything(string message);
}
