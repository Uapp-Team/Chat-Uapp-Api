using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using ChatUapp.Localization;

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
