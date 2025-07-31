using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Core.PermissionManagement.Services;

public class ChatbotPermissionManager : DomainService, ITransientDependency
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly IRepository<ChatbotUserPermission, Guid> _chatbotUserRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;

    public ChatbotPermissionManager(
        IDomainGuidGenerator guidGenerator,
        IRepository<ChatbotUserPermission, Guid> chatbotUserRepository,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant)
    {
        _guidGenerator = guidGenerator;
        _chatbotUserRepository = chatbotUserRepository;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
    }

    public ChatbotUserPermission AssignPermission(
        Guid userId, Guid chatbotId, string permissionName)
    {
        AppGuard.Check(
            !_currentTenant.IsAvailable,
            "User must have a tenant to take permission.");
        return new ChatbotUserPermission(
            _guidGenerator.Create(),
            userId,
            chatbotId,
            permissionName,
            _currentTenant.Id!.Value);
    }

    public async Task<ChatbotUserPermission> UnassignAsync(Guid userId, Guid chatbotId, string permissionName)
    {
        AppGuard.Check(
            !_currentTenant.IsAvailable,
            "User must have a tenant to take permission.");

        var permission = await _chatbotUserRepository.FirstOrDefaultAsync(
            p => p.UserId == userId && p.ChatBotId == chatbotId && p.PermissionName == permissionName);

        Ensure.NotNull(permission, "Permission not found for the specified user and chatbot.");

        return permission!;
    }

    public async Task<bool> CheckAsync(Guid chatbotId, string permissionName)
    {
        //AppGuard.Check(
        //    !_currentUser.IsAuthenticated,
        //    "User is not authenticated. Cannot check permissions without a valid user context.");

        var permission = await _chatbotUserRepository.FirstOrDefaultAsync(
            p => p.UserId == _currentUser.Id && p.ChatBotId == chatbotId && p.PermissionName == permissionName);
        return permission != null;
    }

    public async Task<bool> HasPermissionAsync(Guid chatBotId, string permissionName)
    {
        return await CheckAsync(chatBotId, permissionName);
    }
}
