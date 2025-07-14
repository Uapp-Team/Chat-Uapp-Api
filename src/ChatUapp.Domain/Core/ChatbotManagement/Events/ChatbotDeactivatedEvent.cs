using ChatUapp.Core.ChatbotManagement.Enums;
using System;
using Volo.Abp.Auditing;

namespace ChatUapp.Core.ChatbotManagement.Events;

public class ChatbotDeactivatedEvent : IHasEntityVersion
{
    public Guid ChatboId { get; }
    public string Name { get; } = default!;
    public ChatbotStatus Status { get; }

    public int EntityVersion { get; }

    public ChatbotDeactivatedEvent(Guid chatbotId, string name, ChatbotStatus status)
    {
        ChatboId = chatbotId;
        Name = name;
        Status = status;
    }
}
