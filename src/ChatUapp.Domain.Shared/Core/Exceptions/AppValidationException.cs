using ChatUapp.Core.Constants;
using Volo.Abp;

public class AppValidationException : UserFriendlyException
{
    public AppValidationException(string message)
        : base(message, AppErrorCodes.Validation)
    {
    }
}