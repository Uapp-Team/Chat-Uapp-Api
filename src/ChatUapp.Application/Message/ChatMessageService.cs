using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatUapp.Message.ApiResponsesDtos;
using ChatUapp.Message.Interfaces;
using Refit;
using Volo.Abp.Application.Services;

namespace ChatUapp.Services;

public class ChatMessageService: ApplicationService
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
            if(request.Query == "Hi" || request.Query == "Hello" ||request.Query == "How are you?")
            {
                return new ReplyMessageResponseDto
                {
                    Answer = await AskAsync(request.Query),
                    Success = true,
                    BotName = request.BotName
                }; 
            }
            var reply = await _chatBotEnginerApi.QueryAsync(request.Query, request.BotName, request.Session);
            if (reply.Answer.Contains("Sorry"))
            {
                reply.Answer = await AskAsync(request.Query);
            }
            reply.Answer =  string.IsNullOrWhiteSpace(reply.Answer)? "No result found": reply.Answer;

            return reply;
        }
        catch (System.Exception)
        {
            throw;
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
