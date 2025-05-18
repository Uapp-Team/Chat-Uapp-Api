using ChatUapp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ChatUapp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ChatUappController : AbpControllerBase
{
    protected ChatUappController()
    {
        LocalizationResource = typeof(ChatUappResource);
    }
}
