using ChatUapp.AppIdentity;
using ChatUapp.DTPs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Controllers
{
    [Route("AppAccount")]
    public class ChatAppAccountController : AccountController
    {
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IAccountAppService _accountAppService;
        private readonly IdentityUserManager _userManager;
        public ChatAppAccountController(
            IIdentityUserAppService identityUserAppService,
            IAccountAppService accountAppService,
            IdentityUserManager userManager

           ) : base(accountAppService)
        {
            _identityUserAppService = identityUserAppService;
            _accountAppService = accountAppService;
            _userManager = userManager;
        }

        [HttpPost("app_register")]
        public async Task<IActionResult> Register(AppRegisterDto data)
        {
            var user = new AppIdentityUser(
                GuidGenerator.Create(),
                data.UserName,
                data.EmailAddress
            );
            user.Name = data.FirstName;
            user.Surname = data.LastName;
            user.TitlePrefix = data.TitlePrefix;


            var result = await _userManager.CreateAsync(user, data.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!string.IsNullOrWhiteSpace(data.PhoneNumber))
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, data.PhoneNumber);
                if (!phoneResult.Succeeded)
                {
                    return BadRequest(phoneResult.Errors);
                }
            }
            return Ok(new { Message = "User registered successfully"});
        }
    }
}
