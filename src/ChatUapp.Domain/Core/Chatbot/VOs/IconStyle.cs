using ChatUapp.Core.Guards;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.Chatbot.VOs;

public class IconStyle : ValueObject
{
    public string IconName { get; private set; } = default!;
    public string IconColor { get; private set; } = default!;

    [Obsolete("Only for ORM (EF Core) usage.", true)]
    private IconStyle() { }

    public IconStyle(string iconName, string iconColor)
    {
        Ensure.NotNullOrEmpty(iconName, nameof(iconName));
        Ensure.NotNullOrEmpty(iconColor, nameof(iconColor));

        if (string.IsNullOrWhiteSpace(iconColor))
            throw new ArgumentException("Icon color is required.");

        IconName = iconName;
        IconColor = iconColor;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IconName;
        yield return IconColor;
    }
}
