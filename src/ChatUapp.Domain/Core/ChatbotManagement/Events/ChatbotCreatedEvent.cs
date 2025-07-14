using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.VOs;
using System;

namespace ChatUapp.Core.ChatbotManagement.Events;

public class ChatbotCreatedEvent
{
    public Guid ChatbotId { get; }
    public Guid? TenantId { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Header { get; }
    public string SubHeader { get; }
    public string UniqueKey { get; }
    public IconStyle IconStyle { get; }

    public ChatbotCreatedEvent(Guid id, Guid? tenantId, string name, string? description, string header, string subHeader, string uniqueKey, IconStyle iconStyle)
    {
        ChatbotId = id;
        TenantId = tenantId;
        Name = name;
        Description = description;
        Header = header;
        SubHeader = subHeader;
        UniqueKey = uniqueKey;
        IconStyle = iconStyle;
    }
}
