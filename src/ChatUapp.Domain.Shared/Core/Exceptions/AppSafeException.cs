using ChatUapp.Core.Constants;
using Volo.Abp;

namespace ChatUapp.Core.Exceptions;

public class AppSafeException : UserFriendlyException
{
    public AppSafeException(string message)
        : base(message, AppErrorCodes.SafeError)
    {
    }
}
