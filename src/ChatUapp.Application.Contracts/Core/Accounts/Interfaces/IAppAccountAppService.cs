using ChatUapp.Core.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Core.Accounts.Interfaces;

public interface IAppAccountAppService : IAccountAppService
{
    Task<IdentityUserDto> RegisterAsync(AppRegisterDto input);
}
