using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement.Interfaces
{
    public interface IChatSessionAppService : IApplicationService
    {
        Task<ChatSessionDto> CreateAsync(CreateSessionDto input);
        Task<ChatSessionDto> UpdateAsync(UpdateSessionDto input);
        Task<ChatSessionDto> GetAsync(Guid Id);
        Task<PagedResultDto<ChatSessionTitleDto>> GetTitlesAsync(GetSessionTitlesListDto input);
    }
}
