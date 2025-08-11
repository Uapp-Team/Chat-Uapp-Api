using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.Guards;
using ChatUapp.Core.PermisionManagement.Consts;
using ChatUapp.Core.PermissionManagement.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement;

public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IUserChatSummaryQueryService _userChatSummaryQueryService;
    private readonly ChatbotPermissionManager _permissionManager;

    public DashboardAppService(
        IUserChatSummaryQueryService userChatSummaryQueryService, 
        ChatbotPermissionManager permissionManager)
    {
        _userChatSummaryQueryService = userChatSummaryQueryService;
        _permissionManager = permissionManager;
    }

    public async Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null)
    {
        if (chatbotId.HasValue) {
            var permissionName = ChatbotPermissionConsts.ChatbotAnalyticsView;
            var hasPermission = await _permissionManager.CheckAsync(chatbotId.Value, permissionName);
            AppGuard.HasPermission(hasPermission, permissionName);
        }

        return await _userChatSummaryQueryService.GetDashboardAnalyticsAsync(startDate, endDate, chatbotId);
    }

    public async Task<UserDashboardSummaryDto> GetUserDashboardSummaryAsync(
        DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null)
    {
        var result = await _userChatSummaryQueryService.GetUserDashboardSummariesAsync(startDate, endDate, chatbotId);
        return result;
    }

    public async Task<object> GetChatbotDashboardSummaryAsync(
       DateTime? startDate = null, DateTime? endDate = null, Guid? chatbotId = null)
    {
        if (chatbotId.HasValue)
        {
            var permissionName = ChatbotPermissionConsts.ChatbotDashboardView;
            var hasPermission = await _permissionManager.CheckAsync(chatbotId.Value, permissionName);
            AppGuard.HasPermission(hasPermission, permissionName);
        }
        var result = await _userChatSummaryQueryService.GetChatbotDashboardSummariesAsync(startDate, endDate, chatbotId);
        return result;
    }
}
