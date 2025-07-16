using ChatUapp.Core.Accounts.DTOs;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace ChatUapp.Core.Accounts.Interfaces
{
    public interface IAppProfileAppService : IProfileAppService
    {
        Task<ProfileDto> UpdateAsync(AppUpdateProfileDto input);
        Task<AppProfileDto> GetCurrentUserAsync();
    }
}
