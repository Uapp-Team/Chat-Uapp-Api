using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public abstract class ChatbotPermissionDefinitionProvider
{
    public abstract void Define(ChatbotPermissionDefinitionContext context);
}
