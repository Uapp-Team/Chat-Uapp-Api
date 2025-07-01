using ChatUapp.Accounts.DTOs;
using ChatUapp.Core.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using AppInterfaces = ChatUapp.Accounts.Interfaces;

namespace ChatUapp.Core.Accounts;

[RemoteService(IsEnabled = false)]
[Dependency(ReplaceServices = true)]
[ExposeServices(
typeof(IProfileAppService),
typeof(AppInterfaces.IProfileAppService),
typeof(Volo.Abp.Account.ProfileAppService),
typeof(ProfileAppService)
)]
public class ProfileAppService : Volo.Abp.Account.ProfileAppService, 
    AppInterfaces.IProfileAppService,
    ITransientDependency
{
    public ProfileAppService(IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions
        ) : base(userManager, identityOptions)
    {
    }

    public virtual async Task<Volo.Abp.Account.ProfileDto> UpdateAsync(AppUpdateProfileDto input)
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
