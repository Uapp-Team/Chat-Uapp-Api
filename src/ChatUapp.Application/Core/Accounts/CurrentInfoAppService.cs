using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.Accounts.Interfaces;
using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Interfaces.FileStorage;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Core.Accounts;

public class CurrentInfoAppService : ApplicationService, ICurrentInfoAppService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly IRepository<Chatbot, Guid> _botRepo;
    private readonly IRepository<IdentityRole, Guid> _roleRepo;
    private readonly IIdentityUserRepository _userRepo;
    private readonly IBlobStorageService _storage;

    public CurrentInfoAppService(
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        IRepository<Chatbot, Guid> botRepo,
        IRepository<IdentityRole, Guid> roleRepo,
        IIdentityUserRepository userRepo,
        IBlobStorageService storage)
    {
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _botRepo = botRepo;
        _roleRepo = roleRepo;
        _userRepo = userRepo;
        _storage = storage;
    }

    public async Task<CurrentBotDto> GetCurrentBotId()
    {
        Ensure.Authenticated(_currentUser);

        var queryable = await _botRepo.GetQueryableAsync();
        var chatBot = queryable.Where(x => x.isDefalt == true).FirstOrDefault();
        if (chatBot == null)
            throw new AppBusinessException("No chatbot found for the current user.");

        return ObjectMapper.Map<Chatbot, CurrentBotDto>(chatBot);
    }

    public Task<CurrentTenantDto> GetCurrentTenant()
    {
        Ensure.Authenticated(_currentUser);

        var tenantDto = new CurrentTenantDto
        {
            Id = _currentTenant.Id ?? Guid.Empty,
            Name = _currentTenant.Name ?? string.Empty
        };

        return Task.FromResult(tenantDto);
    }

    public async Task<AppIdentityUserDto> GetCurrentUser()
    {
        Ensure.Authenticated(_currentUser);

        var user = await _userRepo.GetAsync(_currentUser.Id!.Value, includeDetails: true);

        var dto = ObjectMapper.Map<IdentityUser, AppIdentityUserDto>(user);

        // Get custom profile properties
        dto.TitlePrefix = user.GetProperty<string>("TitlePrefix");
        dto.FacebookUrl = user.GetProperty<string>("FacebookUrl");
        dto.InstagramUrl = user.GetProperty<string>("InstagramUrl");
        dto.LinkedInUrl = user.GetProperty<string>("LinkedInUrl");
        dto.TwitterUrl = user.GetProperty<string>("TwitterUrl");
        dto.ProfileImg = user.GetProperty<string>("ProfileImg");

        if(dto.ProfileImg !=null) dto.ProfileImg = await _storage.GetUrlAsync(dto.ProfileImg);
        // Get Role Names
        var roleIds = user.Roles.Select(r => r.RoleId).ToList();

        var roles = await _roleRepo.GetListAsync(r => roleIds.Contains(r.Id));
        dto.Roles = roles.Select(r => r.Name).ToList();

        return dto;
    }
}
