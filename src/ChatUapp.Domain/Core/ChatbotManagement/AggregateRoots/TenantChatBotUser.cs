using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.Guards;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots
{
    public class TenantChatbotUser : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // Properties
        public Guid? TenantId { get; private set; }
        public Guid ChatbotId { get; private set; }
        public Guid UserId { get; private set; }
        public ChatbotUserStatus Status { get; private set; } = ChatbotUserStatus.Active;
        private TenantChatbotUser() { }

        internal TenantChatbotUser(
            Guid id,
            Guid? tenantId,
            Guid chatbotId,
            Guid userId,
            ChatbotUserStatus status) : base(id)
        {
            SetTenantId(tenantId);
            SetChatbotId(chatbotId);
            SetUserId(userId);
            Status = status;
        }

        // Setters with basic validation
        private void SetTenantId(Guid? tenantId)
        {
            Ensure.NotNull(tenantId, nameof(tenantId));
            TenantId = tenantId;
        }

        private void SetChatbotId(Guid chatbotId)
        {
            Ensure.NotNull(chatbotId, nameof(chatbotId));
            ChatbotId = chatbotId;
        }

        private void SetUserId(Guid userId)
        {
            Ensure.NotNull(userId, nameof(userId));
            UserId = userId;
        }
    }
}
