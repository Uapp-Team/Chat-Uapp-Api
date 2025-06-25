using ChatUapp.Accounts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts
{
    public class ApplicationUser : IdentityUserAppService, IApplicationUser
    {
        public ApplicationUser(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IOptions<IdentityOptions> identityOptions, IPermissionChecker permissionChecker) : base(userManager, userRepository, roleRepository, identityOptions, permissionChecker)
        {
        }
        [Authorize(IdentityPermissions.Users.Default)]
        public override Task<IdentityUserDto> GetAsync(Guid id)
        {
            return base.GetAsync(id);
        }

    }
}
