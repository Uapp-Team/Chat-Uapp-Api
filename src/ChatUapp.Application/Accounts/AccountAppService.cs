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
        var user = await base.RegisterAsync(input);
        var identityUser = await UserManager.FindByIdAsync(user.Id.ToString());

        if (identityUser != null)
        {
            identityUser.Name = input?.FirstName;
            identityUser.Surname = input?.LastName;
            identityUser.SetProperty("TitlePrefix", input?.TitlePrefix);

            await UserManager.UpdateAsync(identityUser);
        }

        return user;
    }
}


