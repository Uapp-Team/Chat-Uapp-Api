using ChatUapp.Accounts.DTOs;
using ChatUapp.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Controllers
{
    [Route("AppAccount")]
    public class ChatAppAccountController : AccountController
    {
        private readonly IMyAccountAppService _myAccountAppService;

        public ChatAppAccountController(
            IAccountAppService accountAppService,
            IMyAccountAppService appService
           ) : base(accountAppService)
        {
            _myAccountAppService = appService;
        }

        [HttpPost("app_register")]
        public async Task<IdentityUserDto> Register(AppRegisterDto data)
        {
            return await _myAccountAppService.CustomRegisterAsync(data);
        }
    }
}
