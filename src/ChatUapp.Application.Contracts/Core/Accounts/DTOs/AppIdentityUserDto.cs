using Volo.Abp.Identity;

namespace ChatUapp.Core.Accounts.DTOs
{
    public class AppIdentityUserDto : IdentityUserDto
    {
        public string? TitlePrefix { get; set; }
    }
}
