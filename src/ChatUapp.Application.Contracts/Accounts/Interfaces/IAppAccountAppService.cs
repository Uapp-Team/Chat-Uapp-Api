using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts.Interfaces;

public interface IAppAccountAppService : IAccountAppService
{
    Task<IdentityUserDto> RegisterAsync(AppRegisterDto input);
}
