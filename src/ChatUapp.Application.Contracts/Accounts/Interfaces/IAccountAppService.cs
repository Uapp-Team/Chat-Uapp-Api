using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;

namespace ChatUapp.Accounts.Interfaces;

public interface IAccountAppService : Volo.Abp.Account.IAccountAppService
{
    Task<AppIdentityUserDto> RegisterAsync(AppRegisterDto input);
}
