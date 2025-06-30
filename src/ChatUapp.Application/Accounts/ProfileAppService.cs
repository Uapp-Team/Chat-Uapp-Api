using ChatUapp.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace ChatUapp.Accounts
{
    public class ProfileAppService : Volo.Abp.Account.ProfileAppService, Interfaces.IProfileAppService
    {
        public ProfileAppService(IdentityUserManager userManager, 
            IOptions<IdentityOptions> identityOptions
            ) : base(userManager, identityOptions)
        {
        }

        [RemoteService(false)]
        public override Task<Volo.Abp.Account.ProfileDto> UpdateAsync(Volo.Abp.Account.UpdateProfileDto input)
        {
            return base.UpdateAsync(input);
        }
        public async Task<Volo.Abp.Account.ProfileDto> UpdateAsync(AppUpdateProfileDto input)
        {
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            if (user != null)
            {
                user.SetProperty("TitlePrefix", input.TitlePrefix);
                user.SetProperty("FacebookUrl", input.FacebookUrl);
                user.SetProperty("InstagramUrl", input.InstagramUrl);
                user.SetProperty("LinkedInUrl", input.LinkedInUrl);
                user.SetProperty("TwitterUrl", input.TwitterUrl);
            }
            return await base.UpdateAsync(input);
        }
    }
}
