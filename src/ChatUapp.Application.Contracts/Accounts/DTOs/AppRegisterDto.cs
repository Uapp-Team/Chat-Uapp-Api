﻿using Volo.Abp.Account;

namespace ChatUapp.Accounts.DTOs
{
    public class AppRegisterDto : RegisterDto
    {
        public string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TitlePrefix { get; set; }
    }
}
