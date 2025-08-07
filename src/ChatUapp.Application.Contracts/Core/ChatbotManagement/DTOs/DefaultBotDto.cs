using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs
{
    public class DefaultBotDto
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
