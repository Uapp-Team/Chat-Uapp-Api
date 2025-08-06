using System.Collections.Generic;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;

public class DashboardAnalyticsDto
{
    public List<string> CountryNames { get; set; } = new();
    public List<CountryStatisticsDto> CountryStatistics { get; set; } = new();
}

public class CountryStatisticsDto
{
    public string Name { get; set; } = string.Empty;
    public List<double> Coordinates { get; set; } = new();
    public int Users { get; set; }
    public string Flag { get; set; } = string.Empty;
}