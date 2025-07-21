using ChatUapp.Core.ChatbotManagement.Enums;
using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string Content { get; set; } = default!;
        public MessageType Type { get; set; }
        public DateTime SentAt { get; set; }
    }
}
