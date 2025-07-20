using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class ChatbotManager : DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly IRepository<Chatbot, Guid> _chatbotRepository;

    public ChatbotManager(IDomainGuidGenerator guidGenerator, IRepository<Chatbot, Guid> chatbotRepository)
    {
        _guidGenerator = guidGenerator;
        _chatbotRepository = chatbotRepository;
    }

    public async Task<Chatbot> CreateAsync(
        string name, string header, string subHeader , string iconName, string iconColor)
    {
        if (CurrentTenant.Id == null)
            throw new AppBusinessException("Tenant ID is not set. Ensure you are in a valid tenant context.");

        await HandleDuplicateChatbotAsync(name);

        var bot = new Chatbot(
            _guidGenerator.Create(),
            name,
            header,
            subHeader,
            _guidGenerator.Create().ToString(),
            new IconStyle(iconName, iconColor),
            ChatbotStatus.Active,
            CurrentTenant.Id);

        return bot;
    }

    public async Task UpdateChatbotAsync(
        Chatbot chatbot, string name, string header, string subHeader , string iconName, string iconColor)
    {
        Ensure.NotNull<Chatbot>(chatbot, nameof(chatbot));

        await HandleDuplicateChatbotAsync(name);

        chatbot.SetName(name);
        chatbot.UpdateChatbotStyle(header, subHeader, new IconStyle(iconName, iconColor));
    }

    public void Activate(Chatbot chatbot)
    {
        Ensure.NotNull<Chatbot>(chatbot, nameof(chatbot));
        chatbot.Activate();
    }

    public void Deactivate(Chatbot chatbot)
    {
        Ensure.NotNull<Chatbot>(chatbot, nameof(chatbot));
        chatbot.Deactivate();
    }

    public async Task<Chatbot> CloneAsync(Chatbot chatbot, string newName)
    {
        Ensure.NotNull<Chatbot>(chatbot, nameof(chatbot));
        Ensure.NotNullOrEmpty(newName, nameof(newName));

        await HandleDuplicateChatbotAsync(newName);

        var clonedChatbot = new Chatbot(
            _guidGenerator.Create(),
            newName,
            chatbot.Header,
            chatbot.SubHeader,
            chatbot.UniqueKey,
            chatbot.IconStyle,
            ChatbotStatus.Draft,
            CurrentTenant.Id);

        return clonedChatbot;
    }

    public void Delete(Chatbot chatbot)
    {
        chatbot.IsDeleted = true;
    }

    private async Task HandleDuplicateChatbotAsync(string name)
    {
        if (CurrentTenant.Id == null)
            throw new AppBusinessException("Tenant ID is not set. Ensure you are in a valid tenant context.");

        var isExistName = await _chatbotRepository.AnyAsync(x => x.Name == name && x.TenantId == CurrentTenant.Id);

        if (isExistName)
            throw new AppValidationException("Chatbot with the same name already exists");
    }
}

