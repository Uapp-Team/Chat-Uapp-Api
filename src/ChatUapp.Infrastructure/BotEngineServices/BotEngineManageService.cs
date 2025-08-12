using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Message.ApiResponsesDtos;
using ChatUapp.Core.Message.Interfaces;
using ChatUapp.Core.Thirdparty.Interfaces;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Infrastructure.BotEngineServices;

public class BotEngineManageService : IBotEngineManageService
{
    private readonly IChatBotEngineApi _chatbotEngineApi;
    private readonly IChatGPTApi _chatGptApi;
    private readonly IRepository<ChatSession, Guid> _sessionRepo;

    public BotEngineManageService(
        IChatBotEngineApi chatbotEngineApi, IChatGPTApi chatGptApi, IRepository<ChatSession, Guid> sessionRepo)
    {
        _chatbotEngineApi = chatbotEngineApi;
        _chatGptApi = chatGptApi;
        _sessionRepo = sessionRepo;
    }

    public async Task<BotTrainResponseModel> TrainAsync(BotTrainRequestModel model)
    {
        return await _chatbotEngineApi.TrainTextAsync(model);
    }

    public async Task<BotTrainResponseModel> UpdateTrainAsync(BotTrainRequestModel model)
    {
        return await _chatbotEngineApi.UpdateTrainTextAsync(model);
    }

    public async Task<BotTrainResponseModel> DeleteTrainAsync(string uniqueKey, string botName)
    {
        var result = await _chatbotEngineApi.DeleteDocAsync(uniqueKey, botName);
        if (result.IsSuccessful)
        {
            return new BotTrainResponseModel
            {
                Success = true,
                DocumentCount = 0, // Assuming deletion means no documents left
                CollectionName = botName,
                Message = "Document deleted successfully."
            };
        }
        else
        {
            throw new AppBusinessException($"Failed to delete document: {result.Error?.Message}");
        }
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

            //if (lowerQuery is "hi" or "hello" or "how are you?")
            //{
            //    return new ReplyMessageResponseDto
            //    {
            //        Answer = await AskAsync(query),
            //        Success = true,
            //        BotName = "chatuapp"
            //    };
            //}

            var reply = new ReplyMessageResponseDto { Answer = "", Success = true, BotName = "chatuapp" };//await _chatbotEngineApi.QueryAsync(query, "chatuapp", session);

            //if (reply.Answer?.Contains("Sorry", StringComparison.OrdinalIgnoreCase) == true)
            //{
            reply.Answer = await AskGptAsync(query, Guid.Parse(session));
            //}

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

    public async Task<string> AskGptAsync(string userQuery, Guid sessionId)
    {
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.FirstOrDefault(s => s.Id == sessionId);

        var request = new GptRequestDto
        {
            messages = new List<GptMessage>
            {
                new GptMessage
                {
                    role = "system",
                    content = "You are a University expert. Only answer university students related questions.Don't express that you are a ai of openai. Mention that you are a ai of Uapp"
                },
            }
        };

        if (session != null && session.Messages.Count > 0)
        {
            foreach (var message in session.Messages)
            {
                request.messages.Add(
                    new GptMessage
                    {
                        role = message.Role == MessageRole.User ? "user" : "system",
                        content = message.Content
                    });
            }
        }

        request.messages.Add(new GptMessage
        {
            role = "user",
            content = userQuery
        });

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
