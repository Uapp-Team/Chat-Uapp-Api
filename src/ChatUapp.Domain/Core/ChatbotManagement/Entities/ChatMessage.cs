using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.VOs;
using System;
using Volo.Abp.Domain.Entities;

namespace ChatUapp.Core.ChatbotManagement.Entities;

public class ChatMessage : Entity<Guid>
{
    public Guid SessionId { get; private set; }
    public string Content { get; private set; }
    public MessageRole Role { get; private set; }
    public MessageType Type { get; private set; }
    public DateTime SentAt { get; private set; }

    private ChatMessage() { }

    public ChatMessage(Guid id, Guid sessionId, MessageRole role, string content, MessageType type, DateTime sentAt)
        : base(id)
    {
        SessionId = sessionId;
        Role = role;
        Content = content;
        Type = type;
        SentAt = sentAt;
    }
}