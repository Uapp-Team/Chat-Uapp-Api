using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class ChatSessionTitleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
