using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs
{
    public class ChangeBotStatusDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }
}
