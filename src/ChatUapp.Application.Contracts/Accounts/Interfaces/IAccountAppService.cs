using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts.Interfaces;

public interface IAccountAppService : Volo.Abp.Account.IAccountAppService
{
    Task<IdentityUserDto> RegisterAsync(AppRegisterDto input);
}
