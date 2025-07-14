using ChatUapp.Core.Interfaces.Chatbot;
using System;
using Volo.Abp.Domain.Services;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class ChatbotManager : DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;

    public ChatbotManager(IDomainGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }

    public void CreateChatbot(
        string name, string header, string subHeader, string uniqueKey, string iconName, string iconColor, Guid TenantId)
    {
        //var bot = new Chatbot(
        //    _guidGenerator.Create(),
        //    name,
        //    header,
        //    subHeader,
        //    uniqueKey,
        //    new IconStyle(iconName, iconColor),
        //    TenantId);
    }
}

