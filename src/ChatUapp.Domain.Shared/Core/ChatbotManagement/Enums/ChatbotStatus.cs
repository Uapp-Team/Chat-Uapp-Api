namespace ChatUapp.Core.ChatbotManagement.Enums;

public enum ChatbotStatus
{
    Draft = 0,           // Not yet published
    Active = 1,          // Live and accepting messages
    Inactive = 2,        // Disabled but not deleted
    Archived = 3,        // Old version, kept for history
    Scheduled = 4        // Will be published at a future time
}
