using ChatUapp.Core.Interfaces;

namespace ChatUapp.Infrastructure
{
    public class AskMessageService : IAskMessageService
    {
        public async Task<string> AskAnything(string message)
        {
            await Task.Delay(50);
            return "This message perfectly executed.";
        }
    }
}
