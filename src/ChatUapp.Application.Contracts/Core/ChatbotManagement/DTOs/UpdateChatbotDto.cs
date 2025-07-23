using ChatUapp.Core.ChatbotManagement.Enums;

namespace ChatUapp.Core.ChatbotManagement.DTOs
{
    public class UpdateChatbotDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Header { get; set; } = default!;
        public string SubHeader { get; set; } = default!;
        public ChatbotStatus Status { get; set; }
        public string? BrandImageName { get; set; }
        public string? BrandImageStream { get; set; }
        public string ?iconName { get; set; } = default!;
        public string iconStream  { get; set; } = default!;
        public string iconColor { get; set; } = default!;
    }
}
