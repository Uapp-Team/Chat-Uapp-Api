using System;
using System.Collections.Generic;

namespace ChatUapp.Core.ChatbotManagement.DTOs;

public class UserByChatBotDto
{
    public Guid id { get; set; }
    public string name { get; set; } = default!;
    public string surname { get; set; } = default!;
    public string email { get; set; } = default!;
    public string imageUrl { get; set; } = default!;
    public List<string> Roles { get; set; } = new();
    public string LastModificationTime { get; set; } = default!;
    public string CreationTime { get; set; } = default!;
    public string isAdmin { get; set; } = default!;
    public string profileImg { get; set; } = default!;
}
