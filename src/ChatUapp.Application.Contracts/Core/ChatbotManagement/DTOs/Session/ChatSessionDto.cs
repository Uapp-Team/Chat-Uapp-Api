using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session
{
    public class ChatSessionDto : ExtensibleObject
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public Guid SessionCreator { get; set; }
        public Guid ChatbotId { get; set; }
        public string? Title { get; set; }
        public string? Ip { get; set; }
        public string? BrowserSessionKey { get; set; }

        public List<ChatMessageDto> Messages { get; set; } = new();
    }
}
