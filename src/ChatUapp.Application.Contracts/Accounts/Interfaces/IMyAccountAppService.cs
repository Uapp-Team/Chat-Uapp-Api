using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts.Interfaces;

public interface IMyAccountAppService : IAccountAppService
{
    Task<MyIdentityUserDto> CustomRegisterAsync(AppRegisterDto input);
}
