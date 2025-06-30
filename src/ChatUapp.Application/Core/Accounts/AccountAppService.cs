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

namespace ChatUapp.Core.Accounts;

[RemoteService(IsEnabled = false)]
[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IAccountAppService),
    typeof(ChatUapp.Application...IAccountAppService),
    typeof(IdentityUserAppService),
    typeof(AccountAppService)
)]
public class AccountAppService : Volo.Abp.Account.AccountAppService,
    Interfaces.IAccountAppService,
    ITransientDependency
{
    public AccountAppService(
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
