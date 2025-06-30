using Volo.Abp;

namespace ChatUapp.Core.Exceptions;

public class AppAccessDeniedException : AbpException
{
    public AppAccessDeniedException(string message = "Sorry! You don't have right permission.")
        : base(message)
    {
    }
}
