using Volo.Abp.Identity;

namespace ChatUapp.Accounts.DTOs
{
    public class AppIdentityUserDto : IdentityUserDto
    {
        public string? TitlePrefix { get; set; }
    }
}
