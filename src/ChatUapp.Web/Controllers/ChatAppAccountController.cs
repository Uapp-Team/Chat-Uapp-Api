using ChatUapp.Accounts.DTOs;
using ChatUapp.Accounts.DTOs.ApiRequestsDto;
using ChatUapp.AppIdentity;
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

        [HttpPost("app-register")]
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

        [HttpPost("app-email-verify")]
        public async Task<IActionResult> EmailVerify(VerifyOtpRequestDto otp)
        {
            var user = await _userManager.FindByEmailAsync(otp.Email);

            if (user == null)
                return NotFound(new { Message = "User not found." });

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (otp.Otp != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }

            await _userManager.UpdateAsync(user);

            return Ok(new { Message = "Email verified successfully." });
        }
    }
}
