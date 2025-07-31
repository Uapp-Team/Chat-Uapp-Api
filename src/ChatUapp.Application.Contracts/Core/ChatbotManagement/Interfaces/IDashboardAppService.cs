using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatUapp.Core.ChatbotManagement.Interfaces;

public interface IDashboardAppService
{
    Task<UserDashboardSummaryDto> GetUserDashboardSummaryAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null);

    Task<IList<DashboardAnalyticsDto>> GetDashboardAnalyticsAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null);
}
