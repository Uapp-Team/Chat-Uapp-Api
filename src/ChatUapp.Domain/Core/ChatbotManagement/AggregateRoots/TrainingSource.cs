using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class TrainingSource : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid ChatbotId { get; private set; }

    public string Name { get; private set; } = default!;
    public string? Description { get; set; }

    public TrainingSourceOrigin Origin { get; private set; } = default!;

    public DateTime LastUpdated { get; private set; }

    private TrainingSource() { } // Required by EF Core

    internal TrainingSource(Guid id, Guid chatbotId, string name, TrainingSourceOrigin origin, Guid? tenantId)
        : base(id)
    {
        TenantId = tenantId ?? throw new AppBusinessException("TenantId cannot be null.");
        ChatbotId = chatbotId;
        SetName(name);
        Origin = origin ?? throw new AppBusinessException("Origin must be provided.");
        LastUpdated = DateTime.UtcNow;
    }

    internal void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new AppBusinessException("Name cannot be null or empty.");
        }
        Name = name;
    }

    internal void UpdateOrigin(TrainingSourceOrigin newOrigin)
    {
        Origin = newOrigin ?? throw new AppBusinessException("Origin must be provided.");
        LastUpdated = DateTime.UtcNow;
    }

    internal void UpdateLastUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }
}
