using ChatUapp.Core.ChatbotManagement.DTOs.Invite;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement.Interfaces;

public interface IBotInvitationAppService : IApplicationService
{
    Task<bool> CreateInviteAsync(CreateInviteDto input);
    Task<ValidateTokenResDto> ValidateTokenAsync(string token);
}
