using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class UpdateSessionDto
    {
        public Guid sessionId { get; set; }
        public string sessionTitle { get; set; } = default!;
    }
}
