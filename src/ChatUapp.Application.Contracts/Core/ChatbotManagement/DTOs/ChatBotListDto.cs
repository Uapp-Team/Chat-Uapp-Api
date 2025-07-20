using ChatUapp.Core.ChatbotManagement.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace ChatUapp.Core.ChatbotManagement.DTOs
{
    public class ChatBotListDto : ExtensibleObject
    {
        public Guid id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string Header { get; set; } = default!;
        public string SubHeader { get; set; } = default!;
        public ChatbotStatus Status { get; set; }
        public string? BrandImageName { get; set; } = default!;
        public string? iconName { get; set; } = default!;
        public string? iconColor { get; set; } = default!;
        public int ? satisfactionRate { get; set; } = default!;
        public List<UserInfo> Users { get; set; } = new List<UserInfo>();
    }

    public class UserInfo
    {
        public Guid id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
    }

}
