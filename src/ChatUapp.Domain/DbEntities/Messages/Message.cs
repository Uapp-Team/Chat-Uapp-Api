using System;
using ChatUapp.DbEntities.Messages.VO;
using ChatUapp.Enums;

namespace ChatUapp.DbEntities.Messages;

public class Message : BaseMessage
{
    private Message(Guid? tenantId, MessageText text, MessageType type, Guid? botId, Guid userId, Guid sessionId, bool isLike,string? ip)
    : base(tenantId, text, type, botId, ip)
    {
        UserId = userId;
        SessionId = sessionId;
        IsLike = isLike;
    }

    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public bool IsLike { get; set; }
    public static Message Create(Guid? tenantId, string text, MessageType type, Guid? botId, Guid userId, Guid sessionId, string? ip = null)
    {
        return new Message(tenantId, new MessageText(text), type, botId, userId, sessionId, false, ip);
    }

    public void Like()
    {
        IsLike = true;
        // Raise domain event if needed
    }

    public void Dislike()
    {
        IsLike = false;
        // Raise domain event if needed
    }

    public void ChangeSession(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            throw new ArgumentException("Session ID cannot be empty.");

        SessionId = sessionId;
    }
}
