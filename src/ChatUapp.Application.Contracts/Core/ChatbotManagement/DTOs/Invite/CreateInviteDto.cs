using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Invite;

public class CreateInviteDto
{
    public Guid BotId { get; set; }
    public string UserEmail { get; set; } = default!;
    public string Role { get; set; } = default!;
}

