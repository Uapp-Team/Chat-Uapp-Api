using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace ChatUapp.DbEntities.Messages.VO;

public class MessageText : ValueObject
{
    public string Value { get; }

    public MessageText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Message text cannot be empty.", nameof(value));

        if (value.Length > 5000)
            throw new ArgumentException("Message text exceeds maximum allowed length (5000 characters).", nameof(value));

        Value = value;
    }

    public static implicit operator string(MessageText text) => text.Value;
    public static implicit operator MessageText(string text) => new(text);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
