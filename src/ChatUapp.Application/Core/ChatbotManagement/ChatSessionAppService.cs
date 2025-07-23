using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.MessageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Core.ChatbotManagement;

public class ChatSessionAppService : ApplicationService, IChatSessionAppService
{
    private readonly ChatSessionManager _sessionManager;
    private readonly IRepository<ChatSession, Guid> _sessionRepo;
    private readonly IAskMessageService _message;

    public ChatSessionAppService(
        ChatSessionManager sessionManager,
        IRepository<ChatSession, Guid> sessionRepo,
        IAskMessageService message)
    {
        _sessionManager = sessionManager;
        _sessionRepo = sessionRepo;
        _message = message;
    }

    public async Task<ChatSessionDto> CreateAsync(CreateSessionDto input)
    {
        Ensure.NotNull(input, nameof(input));

        // Create a new chat session instance
        var session = _sessionManager.CreateNewSession(input.userId, input.chatbotId, input.sessionTitle, input.Ip, input.BrowserSessionKey);

        // Send the initial user message to the chatbot and get the response
        var result = await _message.AskAnything(input.sessionTitle);

        // Add the user's message to the session
        _sessionManager.AddMessageToSession(session, input.sessionTitle, MessageRole.User);

        // Add the chatbot's response to the session
        _sessionManager.AddMessageToSession(session, result, MessageRole.Chatbot);

        // Persist the session to the database
        await _sessionRepo.InsertAsync(session);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return the session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<ChatSessionDto> UpdateAsync(UpdateSessionDto input)
    {
        Ensure.NotNull(input, nameof(input));

        // Load the session along with its related messages
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.First(s => s.Id == input.sessionId);
        Ensure.NotNull(session, nameof(session));

        // Send the new message to the chatbot and get the response
        var result = await _message.AskAnything(input.sessionTitle);

        // Add both user and chatbot messages to the session
        _sessionManager.AddMessageToSession(session, input.sessionTitle, MessageRole.User);
        _sessionManager.AddMessageToSession(session, result, MessageRole.Chatbot);

        // Persist the changes to the database
        await _sessionRepo.UpdateAsync(session);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return the updated session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<ChatSessionDto> GetAsync(Guid Id)
    {
        // Load the session along with its related messages
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.First(s => s.Id == Id);

        // Return the updated session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<PagedResultDto<ChatSessionTitleDto>> GetTitlesAsync(GetSessionTitlesListDto input)
    {
        // Retrieve the queryable for chat sessions
        var queryable = await _sessionRepo.GetQueryableAsync();

        // Apply filtering by ChatbotId if specified
        var filteredQuery = queryable
            .Where(x => x.ChatbotId == input.ChatbotId)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Title),
                x => x.Title != null && input.Title != null &&
                     x.Title.ToLower().Trim().Contains(input.Title.ToLower().Trim()));

        // Apply sorting and paging
        var items = await AsyncExecuter.ToListAsync(
            filteredQuery
                .OrderByDescending(x => x.LastModificationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
        );

        // Get total count after filtering
        var totalCount = filteredQuery.Count();

        // Map and return paged result
        return new PagedResultDto<ChatSessionTitleDto>(
            totalCount,
            items.Select(ObjectMapper.Map<ChatSession, ChatSessionTitleDto>).ToList()
        );
    }
}
