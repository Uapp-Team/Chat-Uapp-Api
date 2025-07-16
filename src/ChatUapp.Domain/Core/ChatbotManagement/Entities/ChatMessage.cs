using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Messages.VOs;
using System;
using Volo.Abp.Domain.Entities;

namespace ChatUapp.Core.ChatbotManagement.Entities;

public class ChatMessage : Entity<Guid>
{
    public Guid SessionId { get; private set; }
    public MessageText Content { get; private set; } = default!; // Using MessageText value object for content
    public MessageRole Role { get; private set; } = default!;
    public MessageType Type { get; private set; } = MessageType.Text; // Default to Text type
    public DateTime SentAt { get; private set; }

    private ChatMessage() { }

    public ChatMessage(Guid id, Guid sessionId, MessageRole role, MessageText content, MessageType type, DateTime sentAt)
        : base(id)
    {
        SessionId = sessionId;
        Role = role;
        Content = content;
        Type = type;
        SentAt = sentAt;
    }
}