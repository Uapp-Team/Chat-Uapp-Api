using ChatUapp.Core.Guards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ChatUapp.Core.PermissionManagement;

public class ChatBotUserPermission : FullAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; private set; }
    public Guid ChatBotId { get; private set; }
    public string PermissionName { get; private set; } = string.Empty;
    private ChatBotUserPermission() { } // EF Core
    public ChatBotUserPermission(Guid id, Guid userId, Guid chatBotId, string permissionName)
        : base(id)
    {
        UserId = userId;
        ChatBotId = chatBotId;
        PermissionName = permissionName;
    }
    public void UpdatePermission(string newPermissionName)
    {
        Ensure.NotNullOrEmpty(newPermissionName, nameof(newPermissionName));
        PermissionName = newPermissionName;
    }
}

