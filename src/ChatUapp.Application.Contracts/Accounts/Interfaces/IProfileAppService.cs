
using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace ChatUapp.Accounts.Interfaces
{
    public interface IProfileAppService : Volo.Abp.Account.IProfileAppService
    {
        Task<ProfileDto> UpdateAsync(AppUpdateProfileDto input);
    }
}
