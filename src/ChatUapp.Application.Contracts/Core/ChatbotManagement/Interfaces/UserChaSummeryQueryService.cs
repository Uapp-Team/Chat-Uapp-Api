using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.Interfaces;

public interface IUserChatSummaryQueryService
{
    Task<PagedResultDto<GetAllChatDto>> GetUserChatSummariesAsync(GetAllChatFilterDto filter);
}
