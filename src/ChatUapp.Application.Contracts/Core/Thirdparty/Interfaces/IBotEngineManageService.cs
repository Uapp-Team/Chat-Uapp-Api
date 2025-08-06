using ChatUapp.Core.Message.ApiResponsesDtos;
using System.Threading.Tasks;

namespace ChatUapp.Core.Thirdparty.Interfaces;

public interface IBotEngineManageService
{
    Task<string> AskAnything(string message);
    Task<ReplyMessageResponseDto> AskAnything(string query, string botName, string session);
}
