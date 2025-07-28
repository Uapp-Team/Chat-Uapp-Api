using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class BotInvitationManager : DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly ICurrentUser _user;

    public BotInvitationManager(
        IDomainGuidGenerator guidGenerator,
        ICurrentUser user)
    {
        _guidGenerator = guidGenerator;
        _user = user;
    }

    public async Task<BotInvitation> CreateAsync(Guid botId, string userEmail, string role)
    {
        Ensure.Authenticated(_user);
        Ensure.IsAvailableTenant(CurrentTenant);
        var invitation = new BotInvitation(
            _guidGenerator.Create(),
            _user.Id,
            CurrentTenant.Id,
            botId,
            userEmail,
            role);
        return await Task.FromResult(invitation);
    }

    public async Task<BotInvitation> UpdateInvitationAsync(
        BotInvitation invitation,
        string email,
        string role)
    {
        Ensure.NotNull(invitation, nameof(invitation));

        invitation.UpdateInvitation(email, role);
        return await Task.FromResult(invitation);
    }

    public void MarkAsRegisteredAsync(BotInvitation invitation)
    {
        Ensure.NotNull(invitation, nameof(invitation));

        invitation.MarkAsRegistered();
    }

    public void Unregister(BotInvitation invitation)
    {
        Ensure.NotNull(invitation, nameof(invitation));

        invitation.Unregister();
    }

    public void Delete(BotInvitation invitation)
    {
        Ensure.NotNull(invitation, nameof(invitation));

        invitation.IsDeleted = true;
    }
}
