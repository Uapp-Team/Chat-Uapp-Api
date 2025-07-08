using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ChatUapp.Core.Accounts.AggregateRoots
{
    public class TenantChatbotUser : FullAuditedAggregateRoot<Guid>
    {
        public TenantChatbotUser(
            Guid tenantId, 
            Guid chatbotId, 
            Guid userId, 
            ChatbotUserStatus status)
        {
            TenantId = tenantId;
            ChatbotId = chatbotId;
            UserId = userId;
            Status = status;
        }

        public Guid TenantId { get; private set; }
        public Guid ChatbotId { get; private set; }
        public Guid UserId { get; private set; }
        public ChatbotUserStatus Status { get; set; }
    }
    public enum ChatbotUserStatus
    {
        Active,
        Inactive,
        Pending
    }
}
