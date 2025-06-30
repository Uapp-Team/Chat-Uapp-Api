using Volo.Abp;
using ChatUapp.Core.Constants;

namespace ChatUapp.Core.Exceptions;

public class AppBusinessException : UserFriendlyException
{
    public AppBusinessException(string message)
        : base(message, AppErrorCodes.BusinessRule)
    {
    }
}
