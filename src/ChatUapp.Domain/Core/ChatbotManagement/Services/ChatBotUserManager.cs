using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

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
    }
}
