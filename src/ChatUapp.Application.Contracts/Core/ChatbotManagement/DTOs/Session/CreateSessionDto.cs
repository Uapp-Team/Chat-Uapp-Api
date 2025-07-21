using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class CreateSessionDto 
    {
        public Guid userId { get;  set; }
        public Guid chatbotId { get;  set; }
        public string sessionTitle { get; set; } = default!;
        public string? Ip { get; set; }
        public string? BrowserSessionKey { get; set; }
    }
}
