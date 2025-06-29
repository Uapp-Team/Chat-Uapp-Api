using ChatUapp.Accounts.Interfaces;
using ChatUapp.AppIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp.Identity;


namespace ChatUapp.Accounts
{
    public class AppIdentityUserManager : UserManager<AppIdentityUser>,IAppIdentityUserManager
    {
        public AppIdentityUserManager(IUserStore<AppIdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<AppIdentityUser> passwordHasher,
            IEnumerable<IUserValidator<AppIdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<AppIdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<AppIdentityUser>> logger)
            : base(store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
        }

        public static explicit operator AppIdentityUserManager(IdentityUserManager v)
        {
            throw new NotImplementedException();
        }
    }
}
