
using ChatUapp.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace ChatUapp.Accounts.Interfaces
{
    public interface IAppProfileAppService : IProfileAppService
    {
        Task<ProfileDto> UpdateAsync(AppUpdateProfileDto input);
    }
}
