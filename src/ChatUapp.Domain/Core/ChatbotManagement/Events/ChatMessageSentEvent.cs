using ChatUapp.Core.ChatbotManagement.VOs;
using System;

namespace ChatUapp.Core.ChatbotManagement.Events;

public class ChatMessageSentEvent
{
    public Guid SessionId { get; }
    public Guid MessageId { get; }
    public string Content { get; }
    public MessageRole Role { get; }

    public ChatMessageSentEvent(Guid sessionId, Guid messageId, string content, MessageRole role)
    {
        SessionId = sessionId;
        MessageId = messageId;
        Content = content;
        Role = role;
    }
}
