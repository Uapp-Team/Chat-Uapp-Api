using ChatUapp.Core.Interfaces.MessageServices;

namespace ChatUapp.Infrastructure.MessageServices;

public class AskMessageService : IAskMessageService
{
    public async Task<string> AskAnything(string message)
    {
        await Task.Delay(50);
        return "This message perfectly executed.";
    }
}
