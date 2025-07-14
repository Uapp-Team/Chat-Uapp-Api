using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Guards;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class Chatbot : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Header { get; private set; }
    public string SubHeader { get; private set; }
    public string UniqueKey { get; private set; }
    public IconStyle IconStyle { get; private set; }

    private Chatbot() { } // Required for EF Core

    internal Chatbot(Guid id, string name, string header, string subHeader, string uniqueKey, IconStyle iconStyle, Guid? tenantId)
        : base(id)
    {
        SetTenantId(tenantId);
        SetName(name);
        SetUniqueKey(uniqueKey);
        Header = header;
        SubHeader = subHeader;
        IconStyle = iconStyle;
    }

    private void SetName(string name)
    {
        Ensure.NotNullOrEmpty(name, nameof(name));
        Name = name;
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
}
