using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

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

    public async Task<ChatSession> RenameSession(ChatSession session, string newSessionName)
    {
        session.Title = newSessionName;
        return session;
    }

    public void AddMessageToSession(
        ChatSession session, string content, MessageRole role, MessageType type = MessageType.Text)
    {
        if (session == null)
            throw new ArgumentNullException(nameof(session));
        var messageId = _guidGenerator.Create();
        var sentAtUtc = DateTime.UtcNow;
        session.AddMessage(messageId, sentAtUtc, content, role, type);
    }
}
