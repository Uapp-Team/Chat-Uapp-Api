using System.Collections.Generic;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;

public class UserDashboardSummaryDto
{
    public int TotalMessagesCount { get; set; }
    public int ActiveChatbotsCount { get; set; }
    public int TotalUsersCount { get; set; }
    public List<BotPerformanceDto> BotPerformance { get; set; } = default!;
}

