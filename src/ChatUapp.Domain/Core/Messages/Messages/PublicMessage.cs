using System;
using ChatUapp.Core.Messages.Messages.VO;
using ChatUapp.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

public class PublicMessage : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public PublicMessage(Guid? tenantId, MessageText text, MessageType messageType, Guid? chatBotId, string? ip, string browserSessionKey = "")
    {
        TenantId = tenantId;
        Text = text;
        MessageType = messageType;
        ChatBotId = chatBotId;
        Ip = ip;
        BrowserSessionKey = browserSessionKey;
    }

    public Guid? TenantId { get; protected set; }
    public MessageText Text { get; protected set; } = default!;
    public string? Ip { get; protected set; }
    public string BrowserSessionKey { get; set; } = string.Empty; // Optional, can be used to track user sessions
    public MessageType MessageType { get; protected set; }
    public Guid? ChatBotId { get; protected set; }

    public static PublicMessage Create(
        Guid? tenantId,
        MessageText text,
        MessageType messageType,
        Guid? chatBotId = null,
        string? ip = null)
    {
        return new PublicMessage(tenantId, text, messageType, chatBotId, ip);
    }

    public void UpdateText(string newText)
    {
        Text = new MessageText(newText);
    }

    public void ChangeBot(Guid botId)
    {
        ChatBotId = botId;
    }
}
