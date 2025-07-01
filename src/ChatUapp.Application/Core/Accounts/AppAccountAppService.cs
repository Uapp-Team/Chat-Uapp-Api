using System.Threading.Tasks;
using ChatUapp.Accounts.DTOs;
using ChatUapp.Core.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using ChatUapp.Accounts.Interfaces;

namespace ChatUapp.Core.Accounts;

[RemoteService(IsEnabled = true)]
[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IAccountAppService),
    typeof(IAppAccountAppService),
    typeof(AccountAppService),
    typeof(AppAccountAppService)
)]
public class AppAccountAppService : AccountAppService,
    IAppAccountAppService,
    ITransientDependency
{
    public AppAccountAppService(
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepository,
        IAccountEmailer accountEmailer,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions)
        : base(
            userManager,
            roleRepository,
            accountEmailer,
            identitySecurityLogManager,
            identityOptions)
    {
    }

    [RemoteService(IsEnabled =false)]
    public override Task<IdentityUserDto> RegisterAsync(RegisterDto input)
    {
        return base.RegisterAsync(input);
    }

    public virtual async Task<IdentityUserDto> RegisterAsync(AppRegisterDto input)
    {
        // Call base ABP register logic (creates user, sets password, etc.)
        var userDto = await base.RegisterAsync(input);

        // Retrieve the created identity user from the database
        var identityUser = await UserManager.FindByIdAsync(userDto.Id.ToString());

        // Guard: user should not be null
        Ensure.NotNull(identityUser, nameof(identityUser));

        // Update user properties
        identityUser!.Name = input.FirstName;
        identityUser.Surname = input.LastName;
        identityUser.SetProperty("TitlePrefix", input.TitlePrefix);

        // Save updates
        await UserManager.UpdateAsync(identityUser);

        return userDto;
    }
}
