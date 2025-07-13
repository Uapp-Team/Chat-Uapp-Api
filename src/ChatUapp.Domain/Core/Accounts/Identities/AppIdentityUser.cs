using System;
using Volo.Abp.Identity;

namespace ChatUapp.Core.Accounts.Identities
{
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser(Guid id, string userName, string email)
            : base(id, userName, email)
        {
        }

        public string ? NamePrefex { get; set; }

    }
}
