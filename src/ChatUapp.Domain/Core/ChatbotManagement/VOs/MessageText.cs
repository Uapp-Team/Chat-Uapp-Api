using ChatUapp.Core.Guards;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.Messages.VOs;

public class MessageText : ValueObject
{
    public string Value { get; }

    public MessageText(string value)
    {
        Ensure.NotNullOrEmpty(value, nameof(value));

        if (value.Length > 5000)
            throw new AppValidationException("Message text exceeds maximum allowed length (5000 characters).");

        Value = value;
    }

    public static implicit operator string(MessageText text) => text.Value;
    public static implicit operator MessageText(string text) => new(text);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

