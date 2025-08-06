using ChatUapp.Core.PermissionManagement.Definitions;
using ChatUapp.Core.PermissionManagement.Dtos;
using ChatUapp.Core.PermissionManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        foreach (var group in permisions)
        {
            var perGroup = new ChatbotPermissionGroupDto()
            {
                Name = group.Name,
                DisplayName = group.DisplayName,
                Permissions = new List<ChatbotPermissionDto>()
            };
            foreach (var np in group.Permissions)
            {
                if (np.Children.Count > 0)
                {
                    var perChild = new ChatbotPermissionDto()
                    {
                        Name = np.Name,
                        DisplayName = np.DisplayName,
                    };

                    perChild.Children.AddRange(await MapChildrenAsync(botId, np.Children));
                    perGroup.Permissions.Add(perChild);
                }
                else
                {
                    perGroup.Permissions.Add(await MapAsync(botId, np));
                }
            }
            resutlPermisions.Add(perGroup); // Add the group to the result list
        }

        return resutlPermisions;
    }

    public async Task<IList<ChatbotPermissionGroupDto>> GetMenuByChatBotIdAsync(Guid botId)
    {
        var resutlPermisions = new List<ChatbotPermissionGroupDto>();
        var permisions = ChatbotPermissionRegistry.Groups;

        foreach (var group in permisions)
        {
            var perGroup = new ChatbotPermissionGroupDto()
            {
                Name = group.Name,
                DisplayName = group.MenuDisplayName,
                IsMenu = group.Permissions.Where(x => x.IsGranted).Count() > 0,
                Permissions = new List<ChatbotPermissionDto>()
            };
            foreach (var np in group.Permissions)
            {
                if (np.Children.Count > 0)
                {
                    var perChild = new ChatbotPermissionDto()
                    {
                        Name = np.Name,
                        DisplayName = np.MenuDisplayName,
                        IsMenu = np.IsMenu,
                    };

                    perChild.Children.AddRange(await MapChildrenAsync(botId, np.Children));
                    perGroup.Permissions.Add(perChild);
                }
                else
                {
                    perGroup.Permissions.Add(await MapAsync(botId, np));
                }
            }
            resutlPermisions.Add(perGroup); // Add the group to the result list
        }

        return resutlPermisions;
    }

    private async Task<List<ChatbotPermissionDto>> MapChildrenAsync(Guid botId, List<PermissionDefinition> children)
    {
        var childPermissions = new List<ChatbotPermissionDto>();
        foreach (var child in children)
        {
            childPermissions.Add(await MapAsync(botId, child));
        }
        return childPermissions;
    }

    private async Task<ChatbotPermissionDto> MapAsync(Guid botId, PermissionDefinition p)
    {
        return new ChatbotPermissionDto
        {
            Name = p.Name,
            DisplayName = p.DisplayName,
            IsGranted = await _botPermissionManager.CheckAsync(botId, p.Name),
            IsMenu = p.IsMenu
        };
    }
}
