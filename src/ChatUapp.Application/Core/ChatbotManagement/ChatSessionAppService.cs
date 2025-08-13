using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Extensions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.PermissionManagement.Services;
using ChatUapp.Core.Thirdparty.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace ChatUapp.Core.ChatbotManagement;

[Authorize]
public class ChatSessionAppService : ApplicationService, IChatSessionAppService
{
    private readonly ChatSessionManager _sessionManager;
    private readonly ChatbotPermissionManager _permissionManager;
    private readonly IRepository<ChatSession, Guid> _sessionRepo;
    private readonly IBotEngineManageService _botEngineManageService;
    private readonly ICurrentUser _currentUser;

    public ChatSessionAppService(
        ChatSessionManager sessionManager,
        IRepository<ChatSession, Guid> sessionRepo,
        IBotEngineManageService message,
        ICurrentUser currentUser,
        ChatbotPermissionManager permissionManager)
    {
        _sessionManager = sessionManager;
        _sessionRepo = sessionRepo;
        _botEngineManageService = message;
        _currentUser = currentUser;
        _permissionManager = permissionManager;
    }

    public async Task<ChatSessionDto> CreateAsync(CreateSessionInputDto input)
    {
        Ensure.NotNull(input, nameof(input));

        if (_currentUser.Id == null)
            throw new AppBusinessException("User is not authenticated.");

        var locationSnapshot = new LocationSnapshot(
                input.LocationSnapshot.CountryName,
                input.LocationSnapshot.Longitude,
                input.LocationSnapshot.Latitude,
                input.LocationSnapshot.Flag,
                input.LocationSnapshot.Ip
            );

        // Create a new chat session instance
        var session = _sessionManager.CreateNewSession(_currentUser.Id.Value, input.chatbotId, input.sessionTitle, locationSnapshot, input.BrowserSessionKey);

        // Send the initial user message to the chatbot and get the response
        var result = await _botEngineManageService.AskAnything(
            input.sessionTitle, session.ChatbotId.ToBotName(), session.Id.ToSessionTitle());

        // Add the user's message to the session
        _sessionManager.AddMessageToSession(session, input.sessionTitle, MessageRole.User);

        // Add the chatbot's response to the session
        _sessionManager.AddMessageToSession(session, result.Answer, MessageRole.Chatbot);

        // Persist the session to the database
        await _sessionRepo.InsertAsync(session);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return the session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<ChatSessionDto> UpdateAsync(UpdateSessionInputDto input)
    {
        Ensure.NotNull(input, nameof(input));

        // Load the session along with its related messages
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.First(s => s.Id == input.sessionId);
        Ensure.NotNull(session, nameof(session));

        // Send the new message to the chatbot and get the response
        var result = await _botEngineManageService.AskAnything(input.message, session.ChatbotId.ToString(), session.Id.ToString());

        // Add both user and chatbot messages to the session
        _sessionManager.AddMessageToSession(session, input.message, MessageRole.User);
        _sessionManager.AddMessageToSession(session, result.Answer, MessageRole.Chatbot);

        // Persist the changes to the database
        await _sessionRepo.UpdateAsync(session);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return the updated session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<ChatSessionDto> LikeAsync(LikeDislikeInputDto input)
    {
        Ensure.NotNull(input, nameof(input));

        // Load the session along with its related messages
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.First(s => s.Id == input.sessionId);
        Ensure.NotNull(session, nameof(session));

        // Add both user and chatbot messages to the session
        _sessionManager.LikeMessage(session, input.messageId);

        // Persist the changes to the database
        await _sessionRepo.UpdateAsync(session);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return the updated session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
    }
    public async Task<ChatSessionDto> DislikeAsync(LikeDislikeInputDto input)
    {
        Ensure.NotNull(input, nameof(input));

        // Load the session along with its related messages
        var queryable = await _sessionRepo.WithDetailsAsync(x => x.Messages);

        // Find the specific session by ID
        var session = queryable.First(s => s.Id == input.sessionId);
        Ensure.NotNull(session, nameof(session));

        // Add both user and chatbot messages to the session
        _sessionManager.DislikeMessage(session, input.messageId);

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
        var session = queryable.FirstOrDefault(s => s.Id == Id);

        Ensure.NotNull(session, nameof(session));

        // Return the updated session as a DTO
        return ObjectMapper.Map<ChatSession, ChatSessionDto>(session!);
    }
    public async Task<PagedResultDto<ChatSessionTitleDto>> GetTitlesAsync(GetSessionTitlesListDto input)
    {
        // Retrieve the queryable for chat sessions
        var queryable = await _sessionRepo.GetQueryableAsync();

        // Apply filtering by ChatbotId if specified
        var filteredQuery = queryable
            .Where(x => x.ChatbotId == input.ChatbotId)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Title),
                x => x.Title!.ToLower().Trim().Contains(input.Title!.ToLower().Trim()));

        // Apply sorting and paging
        var items = await AsyncExecuter.ToListAsync(
                 filteredQuery
                .OrderByDescending(x => x.CreationTime)
                .ThenByDescending(x => x.LastModificationTime)
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
    public async Task<bool> DeleteAsync(Guid Id)
    {
        Ensure.NotNull(Id, nameof(Id));

        var session = await _sessionRepo.GetAsync(Id);

        _sessionManager.Delete(session);

        await _sessionRepo.UpdateAsync(session);

        await CurrentUnitOfWork!.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RenameAsync(Guid id, string updatedName)
    {
        Ensure.NotNull(id, nameof(id));

        var session = await _sessionRepo.GetAsync(id);

        _sessionManager.RenameSession(session, updatedName);

        await _sessionRepo.UpdateAsync(session);

        await CurrentUnitOfWork!.SaveChangesAsync();

        return true;
    }
}
