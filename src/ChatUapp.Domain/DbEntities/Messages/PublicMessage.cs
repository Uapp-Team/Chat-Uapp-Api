using System;
using ChatUapp.DbEntities.Messages.VO;
using ChatUapp.Enums;

namespace ChatUapp.DbEntities.Messages;

public class PublicMessage : BaseMessage
{
    public PublicMessage(Guid? tenantId, string text, MessageType type, Guid? botId, string serial, string? ip)
        : base(tenantId, text, type, botId, ip)
    {
        BrowserSessionKey = serial;
    }

    public string BrowserSessionKey { get; set; }

    public static PublicMessage Create(Guid? tenantId, string text, MessageType type, Guid? botId, string browserSessionKey, string? ip = null)
    {
        return new PublicMessage(tenantId, new MessageText(text), type, botId, browserSessionKey, ip);
    }
}
