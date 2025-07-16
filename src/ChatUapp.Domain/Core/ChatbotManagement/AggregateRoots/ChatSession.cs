using ChatUapp.Core.ChatbotManagement.Entities;
using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.Events;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class ChatSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid SessionCreator { get; private set; } = default!;
    public Guid ChatbotId { get; private set; } = default!;
    public string? Title { get; set; }
    public string? Ip { get; set; }
    public string? BrowserSessionKey { get; set; }

    private List<ChatMessage> _messages = new();
    public IReadOnlyCollection<ChatMessage> Messages => _messages;

    private ChatSession() { } // EF

    public ChatSession(
        Guid id, Guid userId,
        Guid chatbotId, string title,
        Guid? tenantId, string? ip = null,
        string? browserSessionKey = null)
        : base(id)
    {
        TenantId = tenantId;
        SessionCreator = userId;
        Title = title;
        ChatbotId = chatbotId;
        Ip = ip;
        BrowserSessionKey = browserSessionKey;
    }

    internal void RenameSession(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new AppBusinessException("Session title cannot be null or empty.");
        }
        Title = newTitle;
    }

    internal void AddMessage(Guid messageId, DateTime sentAtUtc, string content, MessageRole role, MessageType type = MessageType.Text)
    {
        var message = new ChatMessage(messageId, this.Id, role, content, type, sentAtUtc);
        _messages.Add(message);

        AddDistributedEvent(new ChatMessageSentEvent(Id, messageId, content, role));
    }
}
