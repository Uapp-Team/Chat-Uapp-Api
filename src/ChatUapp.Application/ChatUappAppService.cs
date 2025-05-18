using ChatUapp.Localization;
using Volo.Abp.Application.Services;

namespace ChatUapp;

/* Inherit your application services from this class.
 */
public abstract class ChatUappAppService : ApplicationService
{
    protected ChatUappAppService()
    {
        LocalizationResource = typeof(ChatUappResource);
    }
}
