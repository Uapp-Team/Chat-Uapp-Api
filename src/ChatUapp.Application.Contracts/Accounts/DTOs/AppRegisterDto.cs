using System.ComponentModel.DataAnnotations;
using ChatUapp.Core.Constants;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace ChatUapp.Accounts.DTOs
{
    public class AppRegisterDto : RegisterDto
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public required string PhoneNumber { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public required string FirstName { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public required string LastName { get; set; }

        [Required]
        [DynamicStringLength(typeof(AppUserConsts), nameof(AppUserConsts.MaxTitlePrefixLength))]
        public required string TitlePrefix { get; set; }
    }
}
