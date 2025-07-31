using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;

public class BotPerformanceDto
{
    public Guid BotId { get; set; }
    public string BotName { get; set; } = string.Empty;
    public double SatisfactionRate { get; set; }
}
