using ChatUapp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ChatUapp.Web.Pages;

public abstract class ChatUappPageModel : AbpPageModel
{
    protected ChatUappPageModel()
    {
        LocalizationResourceType = typeof(ChatUappResource);
    }
}
