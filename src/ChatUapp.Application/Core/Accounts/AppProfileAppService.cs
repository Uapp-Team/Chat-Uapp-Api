using System.Threading.Tasks;
using ChatUapp.Accounts.DTOs;
using ChatUapp.Accounts.Interfaces;
using ChatUapp.Core.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace ChatUapp.Core.Accounts;

[RemoteService(IsEnabled = true)]
[Dependency(ReplaceServices = true)]
[ExposeServices(
typeof(IProfileAppService),
typeof(IAppProfileAppService),
typeof(ProfileAppService),
typeof(AppProfileAppService)
)]
public class AppProfileAppService : ProfileAppService,
    IAppProfileAppService,
    ITransientDependency
{
    public AppProfileAppService(IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions
        ) : base(userManager, identityOptions)
    {
    }

    [RemoteService(IsEnabled = false)]
    public override Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        return base.UpdateAsync(input);
    }

    public virtual async Task<ProfileDto> UpdateAsync(AppUpdateProfileDto input)
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        Ensure.NotNull(user, nameof(user));

        user.SetProperty("TitlePrefix", input.TitlePrefix);
        user.SetProperty("FacebookUrl", input.FacebookUrl);
        user.SetProperty("InstagramUrl", input.InstagramUrl);
        user.SetProperty("LinkedInUrl", input.LinkedInUrl);
        user.SetProperty("TwitterUrl", input.TwitterUrl);

        return await base.UpdateAsync(input);
    }
}
