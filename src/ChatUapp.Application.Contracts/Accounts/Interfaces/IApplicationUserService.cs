using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts.Interfaces
{
    public interface IApplicationUserService : IIdentityUserAppService
    {
        Task<string> isActive();
    }
}
