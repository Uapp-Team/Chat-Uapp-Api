using ChatUapp.Core.Constants;

namespace ChatUapp.Core.ChatbotManagement.Consts;

public class ChatSessionConsts
{
    public const int TitleMaxLength = AppConsts.NameMaxLength;
    public const int IpMaxLength = AppConsts.IpMaxLength;
    public const int BrowserSessionKeyMaxLength = AppConsts.KeyMaxLength;

    //ChatMessage consts
    public const string MessageColumnType = AppConsts.TextColumnType;
    public const string MessageColumnName = "MessageText";

    //MessageRole consts
    public const int MessageRoleMaxLength = 50;
    public const string MessageRoleColumnName = "Role";
}
