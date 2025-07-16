using ChatUapp.Core.ChatbotManagement.Enums;
using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class TrainingSource : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid ChatbotId { get; private set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? SourceUrl { get; private set; }  // if source type is web then 
    public SourceType SourceType { get; set; } = SourceType.Text;
    public string? sourceFileName { get; set; } // if source type is file then this will be the name of the file stored in bucket
    public string? SouuceFileType { get; set; } // if source type is file then this will be the type of the file stored in bucket
    public DateTime LastUpdated { get; private set; }
    private TrainingSource() { } // Required for EF Core
    internal TrainingSource(Guid id, Guid chatbotId, string name, string sourceUrl, Guid? tenantId)
        : base(id)
    {
        TenantId = tenantId;
        ChatbotId = chatbotId;
        SetName(name);
        SetSourceUrl(sourceUrl);
        LastUpdated = DateTime.UtcNow;
    }
    internal void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }
    internal void SetSourceUrl(string sourceUrl)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
        {
            throw new ArgumentException("Source URL cannot be null or empty.", nameof(sourceUrl));
        }
        SourceUrl = sourceUrl;
    }
    internal void UpdateLastUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }
}

