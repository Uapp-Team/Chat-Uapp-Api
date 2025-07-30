using ChatUapp.Core.PermissionManagement.Definitions;
using ChatUapp.Core.PermissionManagement.Dtos;
using ChatUapp.Core.PermissionManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Core.PermissionManagement.Services;

public class ChatbotPermissionAppService :
    ApplicationService, IChatbotPermissionAppService
{
    private readonly ChatbotPermissionManager _botPermissionManager;
    private readonly IRepository<ChatbotUserPermission, Guid> _repository;

    public ChatbotPermissionAppService(
        IRepository<ChatbotUserPermission, Guid> repository,
        ChatbotPermissionManager botPermissionManager)
    {
        _repository = repository;
        _botPermissionManager = botPermissionManager;
    }

    public async Task AssignAsync(ChatbotPermissionCreateDto input)
    {

        var entity = _botPermissionManager.AssignPermission(input.UserId, input.ChatbotId, input.PermissionName);
        await _repository.InsertAsync(entity, autoSave: true);
    }

    public async Task UnAssignAsync(ChatbotPermissionCreateDto input)
    {
        var entity = await _botPermissionManager.UnassignAsync(input.UserId, input.ChatbotId, input.PermissionName);
        await _repository.DeleteAsync(entity, autoSave: true);
    }

    public async Task<IList<ChatbotPermissionGroupDto>> GetByChatBotIdAsync(Guid botId)
    {
        var resutlPermisions = new List<ChatbotPermissionGroupDto>();
        var permisions = ChatbotPermissionRegistry.Groups;

        foreach (var p in permisions)
        {
            var perGroup = new ChatbotPermissionGroupDto()
            {
                Name = p.Name,
                DisplayName = p.DisplayName,
                Permissions = new List<ChatbotPermissionDto>()
            };
            foreach (var np in p.Permissions)
            {
                if (np.Children.Count > 0)
                {
                    var perChildGroup = new ChatbotPermissionGroupDto()
                    {
                        Name = np.Name,
                        DisplayName = np.DisplayName,
                        Permissions = new List<ChatbotPermissionDto>()
                    };
                    foreach (var child in np.Children)
                    {
                        var per = await MapAsync(botId, child);
                        perGroup.Permissions.Add(per);
                    }
                }
                else
                {
                    perGroup.Permissions.Add(await MapAsync(botId, np));
                }
            }
        }

        return resutlPermisions;
    }

    private async Task<ChatbotPermissionDto> MapAsync(Guid botId, PermissionDefinition p)
    {
        return new ChatbotPermissionDto
        {
            Name = p.Name,
            DisplayName = p.DisplayName,
            IsGranted = await _botPermissionManager.CheckAsync(botId, p.Name)
        };
    }
}
