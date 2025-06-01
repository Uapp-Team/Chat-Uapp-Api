using ChatUapp.Accounts.DTOs;
using ChatUapp.Accounts.Interfaces;
using ChatUapp.AppIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace ChatUapp.Accounts;

public class MyAccountAppService : AccountAppService, IMyAccountAppService, ITransientDependency
{
    public MyAccountAppService(
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepository,
        IAccountEmailer accountEmailer,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions)
        : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
    {
    }

 

    public async Task<MyIdentityUserDto> CustomRegisterAsync(AppRegisterDto input)
    {
        await CheckSelfRegistrationAsync();

        await IdentityOptions.SetAsync();

        var user = new AppIdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress);
        // Add extended fields 
        user.Name = input.FirstName;
        user.Surname = input.LastName;
        user.TitlePrefix = input.TitlePrefix;
       
        input.MapExtraPropertiesTo(user);
        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber);
        await UserManager.SetEmailAsync(user, input.EmailAddress);
        await UserManager.ConfirmEmailAsync(user, await UserManager.GenerateEmailConfirmationTokenAsync(user));
        await UserManager.AddDefaultRolesAsync(user);

        return ObjectMapper.Map<AppIdentityUser, MyIdentityUserDto>(user);
    }
}


