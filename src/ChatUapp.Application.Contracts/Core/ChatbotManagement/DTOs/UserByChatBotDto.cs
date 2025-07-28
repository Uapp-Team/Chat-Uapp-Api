using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs;

public class UserByChatBotDto
{
    public Guid id { get; set; }
    public string name { get; set; } = default!;
    public string avatar { get; set; } = default!;
}
