using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces;
using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Core.ChatbotManagement
{
    public class ChatSessionAppService : ApplicationService, IChatSessionAppService
    {
        private readonly ChatSessionManager _sessionManager;
        private readonly IRepository<ChatSession, Guid> _sessionRepo;
        private readonly IAskMessageService _message;

        public ChatSessionAppService(
            ChatSessionManager sessionManager,
            IRepository<ChatSession, Guid> sessionRepo,
            IAskMessageService message)
        {
            _sessionManager = sessionManager;
            _sessionRepo = sessionRepo;
            _message = message;
        }

        public async Task<ChatSessionDto> CreateAsync(CreateSessionDto input)
        {
            Ensure.NotNull(input, nameof(input));
            var session = _sessionManager.CreateNewSession(input.userId, input.chatbotId, input.sessionTitle, input.Ip, input.BrowserSessionKey);
            var result = await _message.AskAnything(input.sessionTitle);

            _sessionManager.AddMessageToSession(session, result, MessageRole.User);
            _sessionManager.AddMessageToSession(session, result, MessageRole.Chatbot);

            await _sessionRepo.InsertAsync(session);

            await CurrentUnitOfWork!.SaveChangesAsync();

            return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
        }

        public async Task<ChatSessionDto> UpdateAsync(UpdateSessionDto input)
        {
            Ensure.NotNull(input, nameof(input));
            var session = (await _sessionRepo.WithDetailsAsync(x => x.Messages)).FirstOrDefault();
            Ensure.NotNull(session, nameof(session));

            // await _sessionRepo.UpdateAsync(sessions);
            var result = await _message.AskAnything(input.sessionTitle);

            _sessionManager.AddMessageToSession(session, input.sessionTitle, MessageRole.User);
            _sessionManager.AddMessageToSession(session, result, MessageRole.Chatbot);

            await _sessionRepo.UpdateAsync(session);

            await CurrentUnitOfWork!.SaveChangesAsync();

            return ObjectMapper.Map<ChatSession, ChatSessionDto>(session);
        }
    }
}
