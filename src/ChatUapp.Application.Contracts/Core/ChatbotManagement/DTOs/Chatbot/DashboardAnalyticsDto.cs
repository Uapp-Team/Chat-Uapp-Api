using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;

public class DashboardAnalyticsDto
{
    public string Name { get; set; } = string.Empty;
    public List<double> Coordinates { get; set; } = new();
    public int Users { get; set; }
    public string Flag { get; set; } = string.Empty;
}
