using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.Events;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class Chatbot : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; set; } = default!;
    public string Header { get; private set; } = default!;
    public string SubHeader { get; private set; } = default!;
    public string UniqueKey { get; private set; } = default!;
    public ChatbotStatus Status { get; set; } = ChatbotStatus.Draft;
    public string? BrandImageName { get; set; } = default!;
    public bool isDefault {  get; private set; } = false;
    public IconStyle IconStyle { get; private set; } = default!;

    private Chatbot() { } // Required for EF Core

    internal Chatbot(Guid id, string name, string header, string subHeader, string uniqueKey, IconStyle iconStyle, ChatbotStatus status, Guid? tenantId)
        : base(id)
    {
        SetTenantId(tenantId);
        SetName(name);
        SetUniqueKey(uniqueKey);
        Header = header;
        SubHeader = subHeader;
        IconStyle = iconStyle;
        Status = status;
    }

    internal void SetName(string name)
    {
        Ensure.NotNullOrEmpty(name, nameof(name));
        Name = name;
    }

    internal void UpdateChatbotStyle(string header, string subHeader, IconStyle iconStyle)
    {
        Header = header;
        SubHeader = subHeader;
        IconStyle = iconStyle;
    }

    internal void Activate()
    {
        if (Status == ChatbotStatus.Active)
        {
            throw new AppBusinessException("Chatbot is already active.");
        }

        Status = ChatbotStatus.Active;

        AddDistributedEvent(new ChatbotActivatedEvent(Id, Name, Status));     // Publish activation event
    }

    internal void Deactivate()
    {
        if (Status == ChatbotStatus.Inactive)
        {
            throw new AppBusinessException("Chatbot is already inactive.");
        }

        Status = ChatbotStatus.Inactive;

        AddDistributedEvent(new ChatbotDeactivatedEvent(Id, Name, Status));   // Publish deactivation event
    }

    private void SetTenantId(Guid? tenantId)
    {
        Ensure.NotNull(tenantId, nameof(tenantId));
        TenantId = tenantId;
    }

    private void SetUniqueKey(string key)
    {
        Ensure.NotNullOrEmpty(key, nameof(key));
        UniqueKey = key;
    }
    internal void SetDefault()
    {
        isDefault = true;
    }
    internal void SetNotDefault()
    {
        isDefault = false;
    }
}

