using Volo.Abp.Account;


namespace ChatUapp.Core.Accounts.DTOs;

public class AppUpdateProfileDto : UpdateProfileDto
{
    public string? TitlePrefix { get; set; }
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? ProfileImg { get; set; }
    public string? FileName { get; set; }
}
