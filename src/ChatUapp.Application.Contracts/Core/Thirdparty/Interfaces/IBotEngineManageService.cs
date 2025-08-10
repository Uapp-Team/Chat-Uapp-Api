using ChatUapp.Core.Message.ApiResponsesDtos;
using ChatUapp.Core.Message.Interfaces;
using System.Threading.Tasks;

namespace ChatUapp.Core.Thirdparty.Interfaces;

public interface IBotEngineManageService
{
    Task<string> AskAnything(string message);
    Task<ReplyMessageResponseDto> AskAnything(string query, string botName, string session);
    Task<BotTrainResponseModel> TrainAsync(BotTrainRequestModel model);
    Task<BotTrainResponseModel> UpdateTrainAsync(BotTrainRequestModel model);
    Task<BotTrainResponseModel> DeleteTrainAsync(string uniqueKey, string botName);
}
