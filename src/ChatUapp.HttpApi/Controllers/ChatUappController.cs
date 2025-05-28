using ChatUapp.Localization;
using Microsoft.AspNetCore.Identity;
using NUglify.JavaScript.Syntax;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

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
