using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Controllers
{
    [Route("MyAccount")]
    public class MyAccountController : AccountController
    {
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IAccountAppService _accountAppService;

        public MyAccountController(IIdentityUserAppService identityUserAppService, IAccountAppService accountAppService) : base(accountAppService)
        {
            _identityUserAppService = identityUserAppService;
            _accountAppService = accountAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(myRegisterDto data)
        {
            await _accountAppService.RegisterAsync(data);

            var user = await _identityUserAppService.FindByUsernameAsync(data.UserName);
            if(User != null)
            {
                user.Name = data.FirstName;
                user.Surname = data.LastName;
                await _identityUserAppService.UpdateAsync(user.Id,user);
            }
            return Ok(); // Or return CreatedAtAction(...)
        }
    }
    
    public class myRegisterDto : RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
