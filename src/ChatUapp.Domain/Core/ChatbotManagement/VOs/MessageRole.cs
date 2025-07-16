using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.ChatbotManagement.VOs;

public class MessageRole : ValueObject
{
    public string Value { get; private set; } = default!;

    public static MessageRole User => new("user");
    public static MessageRole Assistant => new("assistant");
    public static MessageRole Chatbot => new("chatbot");

    private MessageRole() { }

    private MessageRole(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
