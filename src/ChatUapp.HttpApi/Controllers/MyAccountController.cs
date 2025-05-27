using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ChatUapp.Controllers
{
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
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call your service method
          

           
            return Ok(); // Or return CreatedAtAction(...)
        }
    }
    
    public class RegistarDTo
    {
        public string Email { get; set; }
        // Add other necessary properties like:
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
