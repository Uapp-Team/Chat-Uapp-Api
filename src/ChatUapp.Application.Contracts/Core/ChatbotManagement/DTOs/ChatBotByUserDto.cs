using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs;

public class ChatBotByUserDto
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string IconName { get;  set; } = default!;
    public string IconColor { get;  set; } = default!;
}
