using ChatUapp.Accounts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityUserAppService))]
    public class ApplicationUserService : IdentityUserAppService, IApplicationUserService
    {
        public ApplicationUserService(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IOptions<IdentityOptions> identityOptions, IPermissionChecker permissionChecker) : base(userManager, userRepository, roleRepository, identityOptions, permissionChecker)
        {
        }
        [Authorize(IdentityPermissions.Users.Default)]
        public override Task<IdentityUserDto> GetAsync(Guid id)
        {
            return base.GetAsync(id);
        }

        public Task<string> isActive()
        {
            throw new NotImplementedException();
        }
    }
}
