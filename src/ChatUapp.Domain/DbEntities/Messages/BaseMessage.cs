using System;
using ChatUapp.DbEntities.Messages.VO;
using ChatUapp.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.DbEntities.Messages;

public class BaseMessage : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    private BaseMessage() { }

    public BaseMessage(Guid? tenantId, MessageText text, MessageType type, Guid? botId, string? ip)
    {
        TenantId = tenantId;
        Text = text;
        MessageType = type;
        ChatBotId = botId;
        Ip = ip;
    }

    public Guid? TenantId { get; set; }
    public MessageText Text { get; set; } = string.Empty;
    public string? Ip { get; set; }
    public MessageType MessageType { get; set; }
    public Guid? ChatBotId { get; set; }

    public void UpdateText(string newText)
    {
        // We can raise a domain event here if needed
        Text = newText;
    }

    public void ChangeBot(Guid botId)
    {
        ChatBotId = botId;
    }
}
