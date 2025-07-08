using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ChatUapp.Core.Accounts.AggregateRoots
{
    public class TenantUser : FullAuditedAggregateRoot<Guid>
    {
        public TenantUser(
            Guid tenantId, 
            Guid userId, 
            bool isDefault = false)
        {
            TenantId = tenantId;
            UserId = userId;
            IsDefault = isDefault;
        }

        public Guid TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsDefault { get; set; } = default!;
    }
}
