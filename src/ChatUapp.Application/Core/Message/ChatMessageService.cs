using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Message.ApiResponsesDtos;
using ChatUapp.Core.Message.Interfaces;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.Message;

public class ChatMessageService : ApplicationService
{
    private readonly IChatBotEngineApi _chatBotEnginerApi;
    private readonly IChatGPTApi _chatGptApi;
    public ChatMessageService(IChatBotEngineApi chatBotEnginerApi, IChatGPTApi api)
    {
        _chatBotEnginerApi = chatBotEnginerApi;
        _chatGptApi = api;
    }

    public async Task<ReplyMessageResponseDto> PostMessageAsync([Body] MessageRequest request)
    {
        try
        {
            Ensure.NotNullOrEmpty(request.Query, nameof(request.Query));
            Ensure.NotNullOrEmpty(request.BotName, nameof(request.BotName));

            var lowerQuery = request.Query.Trim().ToLower();

            if (lowerQuery is "hi" or "hello" or "how are you?")
            {
                return new ReplyMessageResponseDto
                {
                    Answer = await AskAsync(request.Query),
                    Success = true,
                    BotName = request.BotName
                };
            }

            var reply = await _chatBotEnginerApi.QueryAsync(request.Query, request.BotName, request.Session);

            if (reply.Answer?.Contains("Sorry", StringComparison.OrdinalIgnoreCase) == true)
            {
                reply.Answer = await AskAsync(request.Query);
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
            Logger.LogError(ex, "Unexpected error during chatbot message handling.");
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

public class MessageRequest
{
    public string Query { get; set; }
    public string BotName { get; set; }
    public string Session { get; set; }
}
