using ChatUapp.Accounts.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
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

        public ProfileAppService(IdentityUserManager userManager,
            IOptions<IdentityOptions> identityOptions
            )
            : base(userManager, identityOptions)
        {

        }

        //[RemoteService(false)]
        public async override Task<ProfileDto> GetAsync()
        {
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            var dto = ObjectMapper.Map<IdentityUser, AppProfileDto>(user);

            // Map extra properties manually
            dto.TitlePrefix = user.GetProperty<string>("TitlePrefix");
            dto.InstagramUrl = user.GetProperty<string>("InstagramUrl");
            dto.LinkedInUrl = user.GetProperty<string>("LinkedInUrl");
            dto.TwitterUrl = user.GetProperty<string>("TwitterUrl");
            dto.FacebookUrl = user.GetProperty<string>("FacebookUrl");

            return dto;
        }
        [RemoteService(false)]
        public override async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
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

           

            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, AppProfileDto>(user);
        }

        //public Task<AppProfileDto> UpdateAsync(AppUpdateProfileDto input)
        //{
        //    throw new NotImplementedException();
        //}
       
        public async Task<ProfileDto> UpdateAsync(AppUpdateProfileDto input)
        {
                await IdentityOptions.SetAsync();

                var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

                // 🔄 Proper concurrency control
                user.ConcurrencyStamp = input.ConcurrencyStamp;

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
                
                //// Set extra fields
                //user.SetProperty("TitlePrefix", input.TitlePrefix);
                //user.SetProperty("InstagramUrl", input.InstagramUrl);
                //user.SetProperty("LinkedInUrl", input.LinkedInUrl);
                //user.SetProperty("TwitterUrl", input.TwitterUrl);
                //user.SetProperty("FacebookUrl", input.FacebookUrl);

                // Optional if extraProperties sent
                input.MapExtraPropertiesTo(user);

                (await UserManager.UpdateAsync(user)).CheckErrors();
                await CurrentUnitOfWork.SaveChangesAsync();

                // Map and return DTO
                var dto = ObjectMapper.Map<IdentityUser, AppProfileDto>(user);
                dto.TitlePrefix = user.GetProperty<string>("TitlePrefix");
                dto.InstagramUrl = user.GetProperty<string>("InstagramUrl");
                dto.LinkedInUrl = user.GetProperty<string>("LinkedInUrl");
                dto.TwitterUrl = user.GetProperty<string>("TwitterUrl");
                dto.FacebookUrl = user.GetProperty<string>("FacebookUrl");

                return dto;
        }
    }
}
