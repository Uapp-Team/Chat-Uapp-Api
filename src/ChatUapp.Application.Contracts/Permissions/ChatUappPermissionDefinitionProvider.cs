using ChatUapp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ChatUapp.Permissions;

public class ChatUappPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ChatUappPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ChatUappPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ChatUappResource>(name);
    }
}
