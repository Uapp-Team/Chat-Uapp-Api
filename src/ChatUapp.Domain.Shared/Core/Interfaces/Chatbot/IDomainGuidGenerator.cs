using System;

namespace ChatUapp.Core.Interfaces.Chatbot;

public interface IDomainGuidGenerator
{
    Guid Create();
}
