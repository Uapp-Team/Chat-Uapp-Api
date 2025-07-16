using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class ChatSessionManager : DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly IRepository<ChatSession, Guid> _chatSessionRepository;

    public ChatSessionManager(IDomainGuidGenerator guidGenerator, IRepository<ChatSession, Guid> chatSessionRepository)
    {
        _guidGenerator = guidGenerator;
        _chatSessionRepository = chatSessionRepository;
    }

    public ChatSession CreateNewSession(Guid userId, Guid chatbotId, string sessionTitle)
    {
        var session = new ChatSession(
            _guidGenerator.Create(),
            userId, chatbotId,
            sessionTitle,
            CurrentTenant.Id);

        return session;
    }

    public ChatSession RenameSession(ChatSession session, string newSessionName)
    {
        session.Rename(newSessionName);
        return session;
    }

    public void AddMessageToSession(
        ChatSession session, string content, MessageRole role, MessageType type = MessageType.Text)
    {
        Ensure.NotNull(session, nameof(session));

        var messageId = _guidGenerator.Create();
        var sentAtUtc = DateTime.UtcNow;
        session.AddMessage(messageId, sentAtUtc, content, role, type);
    }
}
