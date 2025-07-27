using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace ChatUapp.Core.ChatbotManagement.Services
{
    public class ChatBotUserManager : DomainService
    {
        private readonly IDomainGuidGenerator _guidGenerator;

        public ChatBotUserManager(IDomainGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        public Task<TenantChatbotUser> CreateAsync(Guid ChatbotId , Guid UserId)
        {
            if (CurrentTenant.Id == null)
                throw new AppBusinessException("Tenant ID is not set. Ensure you are in a valid tenant context.");

            var obj = new TenantChatbotUser(
                _guidGenerator.Create(),
                CurrentTenant.Id,
                ChatbotId,
                UserId,
                Enums.ChatbotUserStatus.Active);

            return Task.FromResult(obj);
        }

        public Task<TenantChatbotUser> UpdateChatbotAsync(
            TenantChatbotUser tenantBotUser, 
            Guid chatbotId, 
            Guid userId)
        {
            Ensure.NotNull(tenantBotUser, nameof(tenantBotUser));
            tenantBotUser.UpdateChatbotUser(chatbotId, userId);
            return Task.FromResult(tenantBotUser);
        }

        public void Delete(TenantChatbotUser tenentUser)
        {
            tenentUser.IsDeleted = true;
        }

        public void DeleteAll(List<TenantChatbotUser> tenantUsers)
        {
            foreach (var user in tenantUsers)
            {
                user.IsDeleted = true;
            }
        }
    }
}
