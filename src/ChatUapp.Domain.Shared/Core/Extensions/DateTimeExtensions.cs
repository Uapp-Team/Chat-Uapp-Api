using System;

namespace ChatUapp.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToFriendlyDate(this DateTime dateTime)
    {
        return dateTime.ToString("MMMM d, yyyy");
    }

    public static string? ToFriendlyDate(this DateTime? dateTime)
    {
        return dateTime?.ToString("MMMM d, yyyy");
    }
}
