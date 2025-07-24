using ChatUapp.Core.ChatbotManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement.Interfaces
{
    public interface IChatbotAppService : IApplicationService
    {
        Task<ChatbotDto> CreateAsync(CreateChatbotDto input);
        Task<ChatbotDto> CreateCopyAsync(Guid Id);
        Task<ChatbotDto> UpdateAsync(Guid id,UpdateChatbotDto input);
        Task<ChatbotDto> UpdateNameAsync(Guid id,string Name);
        Task<ChatbotDto> GetAsync(Guid id);
        Task<List<ChatBotListDto>> GetAllAsync();
        Task<bool> ChangeStatusAsync(ChangeBotStatusDto input);
    }
}
