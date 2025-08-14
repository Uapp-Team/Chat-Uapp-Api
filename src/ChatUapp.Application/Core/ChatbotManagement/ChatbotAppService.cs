using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs;
using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.FileStorage;
using ChatUapp.Core.PermisionManagement.Consts;
using ChatUapp.Core.PermissionManagement.Definitions;
using ChatUapp.Core.PermissionManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace ChatUapp.Core.ChatbotManagement;

[Authorize]
public class ChatbotAppService : ApplicationService, IChatbotAppService
{
    private readonly ChatbotManager _chatbotManager;
    private readonly ChatbotPermissionManager _permissionManager;
    private readonly ChatBotUserManager _chatbotUserManager;
    private readonly IIdentityUserRepository _userRepo;
    private readonly IRepository<Chatbot, Guid> _botRepo;
    private readonly IUserChatSummaryQueryService _userChatSummaryQueryService;
    private readonly IRepository<TenantChatbotUser, Guid> _tenentBotUserRepo;
    private readonly IBlobStorageService _storage;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<IdentityRole, Guid> _roleRepo;

    public ChatbotAppService(
        ChatbotManager chatbot,
        IRepository<Chatbot, Guid> botRepo,
        IBlobStorageService storage,
        ChatBotUserManager chatbotUserManager,
        ICurrentUser currentUser,
        IRepository<TenantChatbotUser, Guid> tenentBotUserRepo,
        IUserChatSummaryQueryService userChatSummaryQueryService,
        IIdentityUserRepository userRepo,
        ChatbotPermissionManager permissionManager,
        IRepository<IdentityRole, Guid> roleRepo)
    {
        _chatbotManager = chatbot;
        _botRepo = botRepo;
        _storage = storage;
        _chatbotUserManager = chatbotUserManager;
        _currentUser = currentUser;
        _tenentBotUserRepo = tenentBotUserRepo;
        _userChatSummaryQueryService = userChatSummaryQueryService;
        _userRepo = userRepo;
        _permissionManager = permissionManager;
        _roleRepo = roleRepo;
    }

    public async Task<bool> ChangeStatusAsync(ChangeBotStatusDto input)
    {
        var chatbot = await _botRepo.GetAsync(input.Id);

        if (input.IsActive) _chatbotManager.Activate(chatbot);
        else _chatbotManager.Deactivate(chatbot);

        await _botRepo.UpdateAsync(chatbot);
        await CurrentUnitOfWork!.SaveChangesAsync();

        return true;
    }

    public async Task<ChatbotDto> CreateAsync(CreateChatbotDto input)
    {
        Ensure.NotNull(input, nameof(input));
        Ensure.NotNull(input.BrandImageStream, nameof(input.BrandImageStream));
        Ensure.NotNull(input.BrandImageName, nameof(input.BrandImageName));

        input.BrandImageName = await _storage.SaveImagesAsync(input.BrandImageStream!, input.BrandImageName!);
        input.iconName = await _storage.SaveImagesAsync(input.iconStream, input.iconName);

        var chatbot = await _chatbotManager.CreateAsync(
            input.Name,
            input.Header,
            input.SubHeader,
            input.iconName,
            input.iconColor
        );

        if (await _botRepo.CountAsync() <= 0) await _chatbotManager.SetDefaultAsync(chatbot);

        chatbot.BrandImageName = input.BrandImageName;
        chatbot.Description = input.Description;

        Ensure.Authenticated(_currentUser);

        var botUserMaping = await _chatbotUserManager.CreateAsync(chatbot.Id, _currentUser.Id!.Value);

        await _botRepo.InsertAsync(chatbot);

        await _tenentBotUserRepo.InsertAsync(botUserMaping);

        await CurrentUnitOfWork!.SaveChangesAsync();

        return ObjectMapper.Map<Chatbot, ChatbotDto>(chatbot);
    }

    public async Task<ChatbotDto> CreateCopyAsync(Guid id)
    {
        // Retrieve the original chatbot
        var chatbot = await _botRepo.GetAsync(id);

        // Generate a unique name for the copy
        var uniqueSuffix = Guid.NewGuid().ToString().Substring(0, 8); // Or use Random if preferred
        var newName = $"{chatbot.Name}_{uniqueSuffix}";

        // Create a copy using the manager
        var copyChatbot = await _chatbotManager.CreateAsync(
            newName,
            chatbot.Header,
            chatbot.SubHeader,
            chatbot.IconStyle.IconName,
            chatbot.IconStyle.IconColor
        );

        // Copy additional properties
        copyChatbot.BrandImageName = chatbot.BrandImageName;
        copyChatbot.Description = chatbot.Description;

        // Ensure current user is authenticated
        Ensure.Authenticated(_currentUser);

        // Create bot-user mapping with the new chatbot ID
        var botUserMapping = await _chatbotUserManager.CreateAsync(copyChatbot.Id, _currentUser.Id!.Value);

        // Save chatbot and mapping to the repositories
        await _botRepo.InsertAsync(copyChatbot);
        await _tenentBotUserRepo.InsertAsync(botUserMapping);

        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return DTO
        return ObjectMapper.Map<Chatbot, ChatbotDto>(copyChatbot);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Retrieve the chatbot entity by its ID
        var chatbot = await _botRepo.GetAsync(id);
        Ensure.NotNull(chatbot, nameof(chatbot));

        // Retrieve all TenantChatbotUser entities associated with this chatbot
        var query = await _tenentBotUserRepo.GetQueryableAsync();
        var tenetBotUser = query.Where(c => c.ChatbotId == chatbot.Id).ToList();

        // Mark the chatbot entity as deleted (soft delete)
        _chatbotManager.Delete(chatbot);

        // Soft delete all related TenantChatbotUser records
        _chatbotUserManager.DeleteAll(tenetBotUser);

        // Update the chatbot entity in the database
        await _botRepo.UpdateAsync(chatbot);

        // Update all tenant chatbot user records in the database
        await _tenentBotUserRepo.UpdateManyAsync(tenetBotUser);

        // Persist all changes to the database
        await CurrentUnitOfWork!.SaveChangesAsync();

        return true;
    }

    public async Task<List<ChatBotListDto>> GetAllAsync()
    {
        var dtoList = await GetChatBotsByUserInternalAsync(null);

        await MapExistingDataAsync(dtoList);

        return dtoList;
    }

    public async Task<ChatbotDto> GetAsync(Guid id)
    {
        //3a1b2a33-655a-674e-08cb-d3b87275642b
        var chatbot = await _botRepo.GetAsync(id);

        var dto = ObjectMapper.Map<Chatbot, ChatbotDto>(chatbot);

        dto.BrandImageName = await _storage.GetUrlAsync(chatbot.BrandImageName!);
        dto.iconName = await _storage.GetUrlAsync(dto.iconName);

        return dto;
    }

    public async Task<List<ChatBotListDto>> GetAllByUserAsync(Guid userId)
    {
        var dtoList = await GetChatBotsByUserInternalAsync(userId);

        await MapExistingDataAsync(dtoList);

        return dtoList;
    }

    public async Task<ChatbotDto> UpdateAsync(Guid id, UpdateChatbotDto input)
    {
        Ensure.NotNull(input, nameof(input));
        Ensure.NotNull(input.BrandImageStream, nameof(input.BrandImageStream));
        Ensure.NotNull(input.BrandImageName, nameof(input.BrandImageName));

        var chatbot = await _botRepo.GetAsync(id);

        var bot = ObjectMapper.Map<Chatbot, ChatbotDto>(chatbot);

        input.BrandImageName = await _storage.SaveImagesAsync(input.BrandImageStream!, input.BrandImageName!, chatbot.BrandImageName);
        input.iconName = await _storage.SaveImagesAsync(input.iconStream, input.iconName!, bot.iconName);

        var result = await _chatbotManager.UpdateChatbotAsync(
        chatbot,
        input.Name,
        input.Header,
        input.SubHeader,
        input.iconName,
        input.iconColor);

        chatbot.BrandImageName = input.BrandImageName;
        chatbot.Description = input.Description;

        await _botRepo.UpdateAsync(result);

        await CurrentUnitOfWork!.SaveChangesAsync();
        return ObjectMapper.Map<Chatbot, ChatbotDto>(chatbot);
    }

    public async Task<ChatbotDto> UpdateNameAsync(Guid id, string name)
    {
        // Fetch the chatbot from the repository
        var chatbot = await _botRepo.GetAsync(id);
        Ensure.NotNull(chatbot, nameof(chatbot));

        // Update chatbot using the manager (if internal logic exists there)
        var updatedChatbot = await _chatbotManager.UpdateChatbotAsync(
            chatbot,
            name,                               // Updated name
            chatbot.Header,
            chatbot.SubHeader,
            chatbot.IconStyle.IconName,
            chatbot.IconStyle.IconColor
        );

        // Preserve existing properties if not being updated
        updatedChatbot.BrandImageName = chatbot.BrandImageName;
        updatedChatbot.Description = chatbot.Description;

        // Save changes to repository
        await _botRepo.UpdateAsync(updatedChatbot);
        await CurrentUnitOfWork!.SaveChangesAsync();

        // Return mapped DTO
        return ObjectMapper.Map<Chatbot, ChatbotDto>(updatedChatbot);
    }

    public async Task<PagedResultDto<GetAllChatDto>> GetAllChatAsync(GetAllChatFilterDto filter)
    {
        if (filter.ChatbotId.HasValue)
        {
            var permissionName = ChatbotPermissionConsts.ChatbotChatsAllChat;
            var hasPermission = await _permissionManager.CheckAsync(filter.ChatbotId.Value, permissionName);
            AppGuard.HasPermission(hasPermission, permissionName);
        }

        var result = await _userChatSummaryQueryService.GetUserChatSummariesAsync(filter);
        return result;
    }

    public async Task<List<UserByChatBotDto>> GetAllUserByBotAsync(Guid botId)
    {
        var permissionName = ChatbotPermissionConsts.ChatbotBotSettingsManageUsersList;
        var hasPermission = await _permissionManager.CheckAsync(botId, permissionName);

        AppGuard.HasPermission(hasPermission, permissionName);

        return await GetMappedUsersByBotAsync(botId);
    }

    [HttpGet("api/app/chatbot/permissions")]
    public async Task<IReadOnlyList<PermissionGroupDefinition>> GetPermissions(Guid botId)
    {
        var permissionName = ChatbotPermissionConsts.ChatbotBotSettingsManageUsersViewPermission;
        var hasPermission = await _permissionManager.CheckAsync(botId, permissionName);

        AppGuard.HasPermission(hasPermission, permissionName);

        var permisions = ChatbotPermissionRegistry.Groups;

        foreach (var p in permisions)
        {
            foreach (var np in p.Permissions)
            {
                if (np.Children.Count > 0)
                {
                    foreach (var child in np.Children)
                    {
                        child.IsGranted = await _permissionManager.CheckAsync(botId, child.Name);
                    }
                }
                else
                {
                    np.IsGranted = await _permissionManager.CheckAsync(botId, np.Name);
                }
            }
        }
        return permisions;
    }

    public async Task<DefaultBotDto> CreateDefaultAsync(Guid botId)
    {
        var bot = await _botRepo.GetAsync(botId);

        Ensure.NotNull(bot, nameof(bot));

        await _chatbotManager.SetDefaultAsync(bot);

        await _botRepo.UpdateAsync(bot);

        return ObjectMapper.Map<Chatbot, DefaultBotDto>(bot);
    }

    private async Task MapExistingDataAsync(List<ChatBotListDto> dtoList)
    {
        await Task.WhenAll(dtoList.Select(async item =>
        {
            // Last active = last modification time
            //item.lastActive = item.LastModificationTime?.ToString("dd-MM-yyyy");
            item.lastActive = item.LastModificationTime?.ToString();

            // Get bot users
            var botUsers = await GetMappedUsersByBotAsync(item.id);
            item.Users = botUsers.Select(u => new UserInfo
            {
                id = u.id,
                Name = u.name,
                Avatar = u.profileImg
            }).ToList();

            // Map owner from CreatorId if exists
            var creatorUser = item.Users.FirstOrDefault(o => o.id == item.CreatorId);
            if (creatorUser != null)
            {
                item.owner = new Owner
                {
                    id = creatorUser.id,
                    Name = creatorUser.Name,
                    Avatar = creatorUser.Avatar
                };
            }
            else
            {
                // If creator not found, keep owner empty or default
                item.owner = new Owner();
            }
        }));
    }

    private async Task<List<ChatBotListDto>> GetChatBotsByUserInternalAsync(Guid? userId)
    {
        // Use current user ID if null
        var effectiveUserId = userId ?? _currentUser.Id;

        // Get all TenantChatbotUser entries for the given user
        var tenantBotUserQueryable = await _tenentBotUserRepo.GetQueryableAsync();
        var chatbotIds = tenantBotUserQueryable
            .Where(tbu => tbu.UserId == effectiveUserId)
            .Select(tbu => tbu.ChatbotId)
            .Distinct()
            .ToList();

        if (!chatbotIds.Any())
            return new List<ChatBotListDto>();

        // Fetch chatbot entities
        var chatbotQueryable = await _botRepo.GetQueryableAsync();
        var chatbots = chatbotQueryable
            .Where(cb => chatbotIds.Contains(cb.Id))
            .ToList();

        var botsDto = ObjectMapper.Map<List<Chatbot>, List<ChatBotListDto>>(chatbots);

        await MapBotImagesUrlAsync(botsDto);
        // Map to DTO list
        return botsDto;
    }

    private async Task MapBotImagesUrlAsync(List<ChatBotListDto> bots)
    {
        Ensure.NotNull(bots,nameof(bots));

        await Task.WhenAll(bots.Select(async bot =>
        {
            if (!string.IsNullOrEmpty(bot.iconName))
                bot.iconName = await _storage.GetUrlAsync(bot.iconName);

            if (!string.IsNullOrEmpty(bot.BrandImageName))
                bot.BrandImageName = await _storage.GetUrlAsync(bot.BrandImageName);
        }));
    }

    private async Task<List<UserByChatBotDto>> GetMappedUsersByBotAsync(Guid botId)
    {
        // Get all linked users for the chatbot
        var tenantUserQuery = await _tenentBotUserRepo.GetQueryableAsync();
        var userLinks = tenantUserQuery
            .Where(x => x.ChatbotId == botId)
            .ToList();

        var userIds = userLinks.Select(x => x.UserId).Distinct().ToList();
        if (!userIds.Any())
            return new List<UserByChatBotDto>(); // No users found

        // Get all users
        var userQuery = await _userRepo.GetListAsync(includeDetails: true);
        var allUsers = userQuery
            .Where(u => userIds.Contains(u.Id))
            .ToList();

        // Map to DTOs
        var dtos = new List<UserByChatBotDto>();
        foreach (var user in allUsers)
        {
            var dto = ObjectMapper.Map<IdentityUser, UserByChatBotDto>(user);

            // Map profile image URL
            if (user.ExtraProperties.TryGetValue("ProfileImg", out var profileImgObj) &&
                profileImgObj is string blobPath && !string.IsNullOrWhiteSpace(blobPath))
            {
                dto.profileImg = await _storage.GetUrlAsync(blobPath);
            }

            // Map roles
            if (user.Roles != null)
            {
                var roleIds = user.Roles.Select(r => r.RoleId).ToList();
                var roles = await _roleRepo.GetListAsync(r => roleIds.Contains(r.Id));
                dto.Roles = roles.Select(r => r.Name).ToList();
            }

            dtos.Add(dto);
        }

        return dtos;
    }
}
