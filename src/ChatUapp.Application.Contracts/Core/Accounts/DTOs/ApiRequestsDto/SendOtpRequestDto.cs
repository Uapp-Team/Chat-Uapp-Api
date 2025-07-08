using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUapp.Core.Accounts.DTOs.ApiRequestsDto
{
    public class SendOtpRequestDto
    {
        public string Email { get; set; } = string.Empty;
    }
}
