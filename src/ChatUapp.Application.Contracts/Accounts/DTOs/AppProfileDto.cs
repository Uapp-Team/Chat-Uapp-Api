using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace ChatUapp.Accounts.DTOs
{
    public class AppProfileDto : ProfileDto
    {
        public string ?InstagramUrl { get; set; }
        public string ? TitlePrefix { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? FacebookUrl { get; set; }
    }
}
