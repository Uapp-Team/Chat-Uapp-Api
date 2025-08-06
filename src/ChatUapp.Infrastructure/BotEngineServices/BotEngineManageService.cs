using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Message.ApiResponsesDtos;
using ChatUapp.Core.Message.Interfaces;
using ChatUapp.Core.Thirdparty.Interfaces;

namespace ChatUapp.Infrastructure.BotEngineServices;

public class BotEngineManageService : IBotEngineManageService
{
    private readonly IChatBotEngineApi _chatbotEngineApi;
    private readonly IChatGPTApi _chatGptApi;

    public BotEngineManageService(IChatBotEngineApi chatbotEngineApi, IChatGPTApi chatGptApi)
    {
        _chatbotEngineApi = chatbotEngineApi;
        _chatGptApi = chatGptApi;
    }

    public async Task<string> AskAnything(string message)
    {
        await Task.Delay(50);
        return "This message perfectly executed.";
    }

    public async Task<ReplyMessageResponseDto> AskAnything(string query, string botName, string session)
    {
        try
        {
            Ensure.NotNullOrEmpty(query, nameof(query));
            Ensure.NotNullOrEmpty(botName, nameof(botName));
            Ensure.NotNullOrEmpty(session, nameof(session));

            var lowerQuery = query.Trim().ToLower();

            if (lowerQuery is "hi" or "hello" or "how are you?")
            {
                return new ReplyMessageResponseDto
                {
                    Answer = await AskAsync(query),
                    Success = true,
                    BotName = botName
                };
            }

            var reply = await _chatbotEngineApi.QueryAsync(query, botName, session);

            if (reply.Answer?.Contains("Sorry", StringComparison.OrdinalIgnoreCase) == true)
            {
                reply.Answer = await AskAsync(query);
            }

            reply.Answer = string.IsNullOrWhiteSpace(reply.Answer)
                ? "No result found"
                : reply.Answer;

            return reply;
        }
        catch (AppValidationException ex)
        {
            throw new AppSafeException($"Validation failed: {ex.Message}");
        }
        catch (AppBusinessException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            //Logger.LogError(ex, "Unexpected error during chatbot message handling.");
            throw new AppSafeException("Something went wrong while processing your message.");
        }
    }

    public async Task<string> AskAsync(string userQuery)
    {
        var request = new GptRequestDto
        {
            messages = new List<GptMessage>
            {
                new GptMessage
                {
                    role = "system",
                    content = "You are a University expert. Only answer university students related questions."
                },
                new GptMessage
                {
                    role = "user",
                    content = userQuery
                },
            }
        };

        try
        {
            var response = await _chatGptApi.QueryAsync(request);
            return response?.choices.FirstOrDefault()?.message?.content ?? "Sorry I don't know the answer. You can connect with your consultant.";

        }
        catch (System.Exception)
        {
            return "Sorry I don't know please connect with our consultant";
        }
    }
}
