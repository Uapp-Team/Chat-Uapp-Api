using ChatUapp.Core.Interfaces.Chatbot;
using Volo.Abp.Domain.Services;

namespace ChatUapp.Core.Chatbot.Services;

public class ChatbotManager: DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;

    public ChatbotManager(IDomainGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }


}

