using ChatUapp.Core.Accounts.Consts;
using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.Accounts.Interfaces;
using ChatUapp.Core.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

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

    [RemoteService(IsEnabled = false)]
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
        identityUser.SetPhoneNumber(input.PhoneNumber, true);
        identityUser.SetProperty("TitlePrefix", input.TitlePrefix);

        // Assign Role
        if (!await UserManager.IsInRoleAsync(identityUser, SeedDataConsts.Chatbotuser))
        {
            await UserManager.AddToRoleAsync(identityUser, SeedDataConsts.Chatbotuser);
        }

        // Save updates
        await UserManager.UpdateAsync(identityUser);

        return userDto;
    }
}
