namespace ChatUapp.Constants;

public class DbTableNames 
{
    public const string Messages = "Messages";
    public const string ChatBots = "ChatBots";
    public const string TenantUsers = "TenantUsers";
    public const string TenantChatbotUsers = "TenantChatbotUsers";
}

public static class DbSchemaNames
{
    public const string Default = "dbo";
    public const string Tenant = "tnt";
    public const string Messaging = "msg";
}
