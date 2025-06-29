using ChatUapp.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace ChatUapp.Accounts;

public class AccountAppService : Volo.Abp.Account.AccountAppService, Interfaces.IAccountAppService, ITransientDependency
{
    public AccountAppService(
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepository,
        IAccountEmailer accountEmailer,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions)
        : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
    {
    }
    [RemoteService(false)]
    public override Task<IdentityUserDto> RegisterAsync(RegisterDto input)
    {
        return base.RegisterAsync(input);
    }
    public async Task<IdentityUserDto> RegisterAsync(AppRegisterDto input)
    {
        await CheckSelfRegistrationAsync();

        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);
        // Add extended fields 
        user.Name = input.FirstName;
        user.Surname = input.LastName;
        user.SetProperty("TitlePrefix", input.TitlePrefix);
        input.MapExtraPropertiesTo(user);
        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber);
        await UserManager.SetEmailAsync(user, input.EmailAddress);
        await UserManager.ConfirmEmailAsync(user, await UserManager.GenerateEmailConfirmationTokenAsync(user));
        await UserManager.AddDefaultRolesAsync(user);

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

    }
}


