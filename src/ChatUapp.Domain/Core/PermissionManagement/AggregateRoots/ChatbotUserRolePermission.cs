using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.PermissionManagement.AggregateRoots;

public class ChatbotUserRolePermission: FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid UserId { get; private set; }
    public Guid ChatBotId { get; private set; }
    public Guid RoleId { get; private set; }
    public Guid? TenantId { get; private set; }

    private ChatbotUserRolePermission() { }

    internal ChatbotUserRolePermission(Guid userId, Guid chatBotId, Guid roleId, Guid? tenantId)
    {
        UserId = userId;
        ChatBotId = chatBotId;
        RoleId = roleId;
        TenantId = tenantId;
    }

}
