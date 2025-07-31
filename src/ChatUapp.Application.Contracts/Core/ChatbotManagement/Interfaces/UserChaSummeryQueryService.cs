using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.Interfaces;

public interface IUserChatSummaryQueryService
{
    Task<PagedResultDto<GetAllChatDto>> GetUserChatSummariesAsync(GetAllChatFilterDto filter);
    Task<UserDashboardSummaryDto> GetUserDashboardSummariesAsync(
        DateTime? startDate, DateTime? endDate, Guid? chatbotId);

    Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync(
        DateTime? startDate, DateTime? endDate, Guid? chatbotId);
}
