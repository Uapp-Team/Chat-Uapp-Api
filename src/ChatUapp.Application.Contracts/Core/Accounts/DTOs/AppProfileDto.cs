using Volo.Abp.Account;

namespace ChatUapp.Core.Accounts.DTOs
{
    public class AppProfileDto : ProfileDto
    {
        public string? ProfileImgUrl { get; set; }
    }
}
