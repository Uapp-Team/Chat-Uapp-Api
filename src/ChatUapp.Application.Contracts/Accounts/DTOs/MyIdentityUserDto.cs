using Volo.Abp.Identity;

namespace ChatUapp.Accounts.DTOs
{
    public class MyIdentityUserDto : IdentityUserDto
    {
        public string? TitlePrefix { get; set; }
    }
}
