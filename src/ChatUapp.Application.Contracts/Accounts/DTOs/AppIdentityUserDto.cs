using Volo.Abp.Identity;

namespace ChatUapp.Accounts.DTOs
{
    public class AppIdentityUserDto : IdentityUserDto
    {
        public string? NamePrefex { get; set; }
    }
}
