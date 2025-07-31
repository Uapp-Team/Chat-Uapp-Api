using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement;

public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IUserChatSummaryQueryService _userChatSummaryQueryService;

    public DashboardAppService(IUserChatSummaryQueryService userChatSummaryQueryService)
    {
        _userChatSummaryQueryService = userChatSummaryQueryService;
    }

    public Task<object> GetDashboardAnalyticsAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDashboardSummaryDto> GetUserDashboardSummaryAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null)
    {
        var result = await _userChatSummaryQueryService.GetUserDashboardSummariesAsync(startDate, endDate, chatbotId);
        return result;
    }
}
