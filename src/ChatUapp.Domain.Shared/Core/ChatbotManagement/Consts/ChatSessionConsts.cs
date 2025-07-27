using ChatUapp.Core.Constants;

namespace ChatUapp.Core.ChatbotManagement.Consts;

public class ChatSessionConsts
{
    public const int TitleMaxLength = AppConsts.NameMaxLength;
    public const int BrowserSessionKeyMaxLength = AppConsts.KeyMaxLength;

    //ChatMessage consts
    public const string MessageColumnType = AppConsts.TextColumnType;
    public const string MessageColumnName = "MessageText";

    //MessageRole consts
    public const int MessageRoleMaxLength = 50;
    public const string MessageRoleColumnName = "Role";

    // LocationSnapshot consts
    public const int CountryNameMaxLength = 100;
    public const int FlagMaxLength = 20;
    public const int IpMaxLength = AppConsts.IpMaxLength;
    public const string LongitudePrecision = "double precision";
    public const string LattitudePrecision = "double precision";
    public const string CountryNameColumnName = "CountryName";
    public const string LatitudeColumnName = "Latitude";
    public const string LongitudeColumnName = "Longitude";
    public const string FlagColumnName = "Flag";
    public const string IpColumnName = "IpAddress";
}
