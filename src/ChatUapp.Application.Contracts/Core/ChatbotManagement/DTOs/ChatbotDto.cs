using ChatUapp.Core.ChatbotManagement.Enums;
using Volo.Abp.ObjectExtending;

namespace ChatUapp.Core.ChatbotManagement.DTOs
{
    public class ChatbotDto : ExtensibleObject
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string Header { get; set; } = default!;
        public string SubHeader { get; set; } = default!;
        public ChatbotStatus Status { get; set; }
        public string? BrandImageName { get; set; } = default!;
        public string iconName { get; set; } = default!;
        public string iconColor { get; set; } = default!;
    }
}
