using ChatUapp.Core.Guards;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.PermissionManagement;

public class ChatbotUserPermission : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid UserId { get; private set; }
    public Guid ChatBotId { get; private set; }
    public string PermissionName { get; private set; } = string.Empty;

    public Guid? TenantId { get; private set; }

    private ChatbotUserPermission() { } // EF Core
    public ChatbotUserPermission(Guid id, Guid userId, Guid chatBotId, string permissionName, Guid tenantId)
        : base(id)
    {
        UserId = userId;
        ChatBotId = chatBotId;
        PermissionName = permissionName;
        TenantId = tenantId;
    }
    public void UpdatePermission(string newPermissionName)
    {
        Ensure.NotNullOrEmpty(newPermissionName, nameof(newPermissionName));
        PermissionName = newPermissionName;
    }
}

