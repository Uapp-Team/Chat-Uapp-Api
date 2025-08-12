using ChatUapp.Core.Interfaces.Chatbot;
using ChatUapp.Core.PermissionManagement.AggregateRoots;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Core.PermissionManagement.Services;

public class ChatbotUserRolePermissionManager : DomainService, ITransientDependency
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly IRepository<ChatbotUserRolePermission, Guid> _chatbotUserRoleRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;

    public ChatbotUserRolePermissionManager(
        IDomainGuidGenerator guidGenerator, 
        IRepository<ChatbotUserRolePermission, Guid> chatbotUserRoleRepository, 
        ICurrentUser currentUser, 
        ICurrentTenant currentTenant)
    {
        _guidGenerator = guidGenerator;
        _chatbotUserRoleRepository = chatbotUserRoleRepository;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
    }
}
