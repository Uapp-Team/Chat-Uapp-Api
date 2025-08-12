using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs.Invite;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.Emailing;
using ChatUapp.Core.PermisionManagement.Consts;
using ChatUapp.Core.PermissionManagement.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement;

public class BotInvitationAppService : ApplicationService, IBotInvitationAppService
{
    private readonly BotInvitationManager _botInvitatioManager;
    private readonly ChatBotUserManager _chatbotUserManager;
    private readonly ChatbotPermissionManager _botParmissionManager;
    private readonly IRepository<BotInvitation, Guid> _botInvitationRepo;
    private readonly IRepository<TenantChatbotUser, Guid> _tenentBotUserRepo;
    private readonly IIdentityUserRepository _userRepo;
    private readonly IAppEmailSender _emailSender;
    private readonly IDataFilter<IMultiTenant> _multitenant;


    public BotInvitationAppService(
        BotInvitationManager botInvitatioManager,
        IRepository<BotInvitation, Guid> botInvitationRepo,
        IRepository<TenantChatbotUser, Guid> tenentBotUserRepo,
        IAppEmailSender emailSender,
        IIdentityUserRepository userRepo,
        ChatBotUserManager chatbotUserManager,
        IDataFilter<IMultiTenant> multitenant,
        ChatbotPermissionManager botParmissionManager)
    {
        _botInvitatioManager = botInvitatioManager;
        _botInvitationRepo = botInvitationRepo;
        _tenentBotUserRepo = tenentBotUserRepo;
        _emailSender = emailSender;
        _userRepo = userRepo;
        _chatbotUserManager = chatbotUserManager;
        _multitenant = multitenant;
        _botParmissionManager = botParmissionManager;
    }

    public async Task<bool> CreateInviteAsync(CreateInviteDto input)
    {
        Ensure.NotNull(input, nameof(input));

        var permissionName = ChatbotPermissionConsts.ChatbotBotSettingsManageUsersInviteUser;
        var hasPermission = await _botParmissionManager.CheckAsync(input.BotId, permissionName);

        AppGuard.HasPermission(hasPermission, permissionName);

        var invited = await _botInvitatioManager.CreateAsync(input.BotId, input.UserEmail, input.Role);

        var acceptUrl = $"http://localhost:3000/ValidateUser?token={invited.InvitationToken}";


        var emailBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
          <style>
            body {{
              font-family: Arial, sans-serif;
              background-color: #f4f4f4;
              color: #333;
              padding: 20px;
            }}
            .container {{
              background-color: #ffffff;
              padding: 20px;
              border-radius: 8px;
              max-width: 600px;
              margin: auto;
              box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            }}
            .button {{
              background-color: #4CAF50;
              color: white;
              padding: 12px 20px;
              text-decoration: none;
              border-radius: 5px;
              display: inline-block;
              margin-top: 20px;
            }}
            .footer {{
              font-size: 12px;
              color: #777;
              margin-top: 30px;
            }}
          </style>
        </head>
        <body>
          <div class='container'>
            <h2>You're Invited to Join the Bot</h2>
            <p>Hello,</p>
            <p>You’ve been invited to collaborate on a bot in our platform with the role of <strong>{input.Role}</strong>.</p>
            <p>Please click the button below to accept the invitation and complete your registration:</p>
            <a href='{acceptUrl}' class='button'>Accept Invitation</a>
            <p class='footer'>If you did not expect this email, you can safely ignore it.</p>
          </div>
        </body>
        </html>";

        await _emailSender.SendAsync(
            to: input.UserEmail,
            subject: "You've been invited to join a bot",
            body: emailBody,
            isBodyHtml: true // important!
        );

        await _botInvitationRepo.InsertAsync(invited);
        await CurrentUnitOfWork!.SaveChangesAsync();

        return true;
    }

    public async Task<ValidateTokenResDto> ValidateTokenAsync(string token)
    {
        Ensure.NotNull(token, nameof(token));

        var result = new ValidateTokenResDto();

        // Disable tenant filter to find the invitation
        using (_multitenant.Disable())
        {
            // Step 1: Find Invitation by Token
            var invitation = await _botInvitationRepo.FindAsync(x => x.InvitationToken == token);

            if (invitation == null)
            {
                result.IsValid = false;
                return result;
            }

            // Step 2: Check if user is registered
            var user = await _userRepo.FindByNormalizedEmailAsync(invitation.UserEmail.ToUpper());

            if (user == null)
            {
                result.IsValid = false;
                return result;
            }

            AppGuard.Check(invitation.TenantId is null, "Tenant id can not be null");

            // Step 3: Map User to Bot
            var mapping = await _chatbotUserManager.CreateAsync(invitation.BotId, user.Id, invitation.TenantId!.Value);

            // Step 4: Save mapping
            await _tenentBotUserRepo.InsertAsync(mapping);

            // Step 5: Clean up the invitation
            _botInvitatioManager.Delete(invitation);

            await _botInvitationRepo.UpdateAsync(invitation);
        }

        // Step 6: Commit transaction
        await CurrentUnitOfWork!.SaveChangesAsync();

        result.IsValid = true;
        return result;
    }
}
