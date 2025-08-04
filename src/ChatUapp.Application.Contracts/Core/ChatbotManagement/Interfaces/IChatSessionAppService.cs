using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement.Interfaces
{
    public interface IChatSessionAppService : IApplicationService
    {
        Task<ChatSessionDto> CreateAsync(CreateSessionInputDto input);
        Task<ChatSessionDto> UpdateAsync(UpdateSessionInputDto input);
        Task<ChatSessionDto> GetAsync(Guid Id);
        Task<bool> DeleteAsync(Guid Id);
        Task<PagedResultDto<ChatSessionTitleDto>> GetTitlesAsync(GetSessionTitlesListDto input);
    }
}
