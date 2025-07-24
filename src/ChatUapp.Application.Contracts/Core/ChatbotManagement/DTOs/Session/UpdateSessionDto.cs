using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class UpdateSessionDto
    {
        public Guid sessionId { get; set; }
        public string message { get; set; } = default!;
    }
}
