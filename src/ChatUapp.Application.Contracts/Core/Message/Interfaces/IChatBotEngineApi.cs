using ChatUapp.Core.Message.ApiResponsesDtos;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatUapp.Core.Message.Interfaces;

public interface IChatBotEngineApi
{
    [Get("/advanced-query?query={query}&botName={botName}&session={session}")]
    Task<ReplyMessageResponseDto> QueryAsync(string query, string botName, string session);

    [Post("/train-text")]
    Task<BotTrainResponseModel> TrainTextAsync([Body] BotTrainRequestModel request);
}

public interface IChatGPTApi
{
    [Post("/v1/chat/completions")]
    Task<GptResponseDto> QueryAsync([Body] GptRequestDto request);
}

public class BotTrainRequestModel
{
    public string Text { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string BotName { get; set; } = string.Empty;
    public int ChunkSize { get; set; } = 1000;
    public int ChunkOverlap { get; set; } = 150;
}

public class BotTrainResponseModel
{
    public bool Success { get; set; }
    public int DocumentCount { get; set; }
    public string CollectionName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class GptRequestDto
{
    public string model { get; set; } = "gpt-4"; // or "gpt-3.5-turbo"
    public List<GptMessage> messages { get; set; } = default!;
    public double temperature { get; set; } = 0.7;
}

public class GptMessage
{
    public string role { get; set; } = default!;
    public string content { get; set; } = default!;
}

public class GptResponseDto
{
    public List<Choice> choices { get; set; } = default!;

    public class Choice
    {
        public GptMessage message { get; set; } = default!;
    }
}
