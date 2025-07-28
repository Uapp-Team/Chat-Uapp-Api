using ChatUapp.Core.ChatbotManagement.Entities;
using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.Events;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Messages.VOs;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class ChatSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid SessionCreator { get; private set; }
    public Guid ChatbotId { get; private set; }
    public string? Title { get; private set; }
    public LocationSnapshot LocationSnapshot { get; private set; } = default!;
    public string? BrowserSessionKey { get; private set; }

    private readonly List<ChatMessage> _messages = new();
    public IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();

    private ChatSession() { } // EF Core

    public ChatSession(Guid id, Guid userId, Guid chatbotId, string? title, Guid? tenantId, LocationSnapshot snapshot, string? browserSessionKey = null)
        : base(id)
    {
        TenantId = tenantId;
        SessionCreator = userId;
        Title = title;
        ChatbotId = chatbotId;
        LocationSnapshot = snapshot;
        BrowserSessionKey = browserSessionKey;
    }

    internal void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new AppBusinessException("Session title cannot be empty.");
        }
        Title = newTitle;
    }

    internal void AddMessage(Guid messageId, DateTime sentAtUtc, MessageText content, MessageRole role, MessageType type = MessageType.Text)
    {
        var message = new ChatMessage(messageId, Id, role, content, type, sentAtUtc);
        _messages.Add(message);

        AddDistributedEvent(new ChatMessageSentEvent(Id, messageId, content.Value, role));
    }

    internal void ReactMessage(Guid messageId, ReactType reactType)
    {
        var message = _messages.Find(m => m.Id == messageId);
        if (message == null)
        {
            throw new AppBusinessException("Message not found.");
        }
        message.SetReactType(reactType);
    }
}
