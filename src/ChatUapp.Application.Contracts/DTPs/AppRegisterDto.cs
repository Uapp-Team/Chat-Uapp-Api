
using Volo.Abp.Account;

namespace ChatUapp.DTPs
{
    public class AppRegisterDto : RegisterDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TitlePrefix { get; set; }
    }
}
