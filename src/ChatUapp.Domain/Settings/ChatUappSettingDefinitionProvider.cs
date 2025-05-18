using Volo.Abp.Settings;

namespace ChatUapp.Settings;

public class ChatUappSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ChatUappSettings.MySetting1));
    }
}
