using ChatUapp.Accounts.DTOs;
using ChatUapp.Accounts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace ChatUapp.Accounts
{
    public class ProfileAppService : Volo.Abp.Account.ProfileAppService, Interfaces.IProfileAppService
    {
        public ProfileAppService(IdentityUserManager userManager, IOptions<IdentityOptions> identityOptions) : base(userManager, identityOptions)
        {
        }
        [RemoteService(false)]
        public override Task<Volo.Abp.Account.ProfileDto> UpdateAsync(Volo.Abp.Account.UpdateProfileDto input)
        {
            return base.UpdateAsync(input);
        }
        public async Task<Volo.Abp.Account.ProfileDto> UpdateAsync(AppUpdateProfileDto input)
        {
            await IdentityOptions.SetAsync();
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
            user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
            if (!string.Equals(user.UserName, input.UserName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
                {
                    (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
                }
            }

            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
                {
                    (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
                }
            }

            if (user.PhoneNumber.IsNullOrWhiteSpace() && input.PhoneNumber.IsNullOrWhiteSpace())
            {
                input.PhoneNumber = user.PhoneNumber;
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }
            user.Name = input.Name?.Trim();
            user.Surname = input.Surname?.Trim();
            user.SetProperty("TitlePrefix", input.TitlePrefix);
            user.SetProperty("FacebookUrl", input.FacebookUrl);
            user.SetProperty("InstagramUrl", input.InstagramUrl);
            user.SetProperty("LinkedInUrl", input.LinkedInUrl);
            user.SetProperty("TwitterUrl", input.TwitterUrl);
            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
            
        }
    }
}
