using System.Collections.Generic;
using Volo.Abp.Identity;

namespace ChatUapp.Core.Accounts.DTOs;

public class AppIdentityUserDto : IdentityUserDto
{
    public string? TitlePrefix { get; set; }
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? ProfileImg { get; set; }
    public List<string> Roles { get; set; } = new();
}
