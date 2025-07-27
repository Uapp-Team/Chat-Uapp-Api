using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.Accounts.Interfaces;
using ChatUapp.Core.Guards;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace ChatUapp.Core.Accounts;

public class CurrentInfoAppService : ApplicationService, ICurrentInfoAppService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;

    public CurrentInfoAppService(
        ICurrentUser currentUser,
        ICurrentTenant currentTenant)
    {
        _currentUser = currentUser;
        _currentTenant = currentTenant;
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

    public Task<CurrentUserDto> GetCurrentUser()
    {
        Ensure.Authenticated(_currentUser);

        var userDto = new CurrentUserDto
        {
            Id = _currentUser.Id ?? Guid.Empty,
            UserName = _currentUser.UserName ?? string.Empty,
            Email = _currentUser.Email ?? string.Empty,
            Name = _currentUser.Name ?? string.Empty,
            Surname = _currentUser.SurName ?? string.Empty,
            Roles = _currentUser.Roles ?? Array.Empty<string>()
        };

        return Task.FromResult(userDto);
    }
}
