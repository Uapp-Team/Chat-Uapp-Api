using ChatUapp.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ChatUapp.Web;

[Dependency(ReplaceServices = true)]
public class ChatUappBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ChatUappResource> _localizer;

    public ChatUappBrandingProvider(IStringLocalizer<ChatUappResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
