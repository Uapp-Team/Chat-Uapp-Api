using ChatUapp.Core.Constants;
using Volo.Abp;

namespace ChatUapp.Core.Exceptions;

public class AppBusinessException : UserFriendlyException
{
    public AppBusinessException(string message)
        : base(message, AppErrorCodes.BusinessRule)
    {
    }
}
