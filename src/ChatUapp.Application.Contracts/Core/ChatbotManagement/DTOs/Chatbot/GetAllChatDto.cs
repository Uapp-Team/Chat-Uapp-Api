using System;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;

public class GetAllChatDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public string BotName { get; set; } = string.Empty;
    public Guid BotId { get; set; }
    public int SessionCount { get; set; }
    public string LastMessage { get; set; } = string.Empty;
}

public class GetAllChatFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? ChatbotId { get; set; }
    public string? Text { get; set; }
    public string? Country { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}
