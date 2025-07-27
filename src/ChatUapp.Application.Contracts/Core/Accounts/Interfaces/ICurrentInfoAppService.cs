using ChatUapp.Core.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.Accounts.Interfaces;

public interface ICurrentInfoAppService : IApplicationService
{
    Task<CurrentUserDto> GetCurrentUser();
    Task<CurrentTenantDto> GetCurrentTenant();
}
