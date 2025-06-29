using System;
using Volo.Abp.Identity;

namespace ChatUapp.AppIdentity
{
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser(Guid id, string userName, string email)
            : base(id, userName, email)
        {
        }
        public string ? TitlePrefix { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? FacebookUrl { get; set; }

    }
}
