using Auth.Data;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountsController : ControllerBase
    {

        private readonly ILogger<AccountsController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IManageJWTService _jwtService;

        public AccountsController(ILogger<AccountsController> logger, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IManageJWTService jwtService)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Create([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                var claims = new List<Claim>()
                {
                    new Claim("email", userCredentials.Email),
                    new Claim("userId", user.Id)
                };
                var claimsDB = await userManager.GetClaimsAsync(user);
                claims.AddRange(claimsDB);

                return _jwtService.BuildToken(userCredentials,
                    claims,
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSetting:key"])),
                    DateTime.UtcNow.AddYears(1));
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {

                var user = await userManager.FindByEmailAsync(userCredentials.Email);

                var claims = new List<Claim>()
                {
                    new Claim("email", userCredentials.Email),
                    new Claim("userId", user.Id)
                };
                var claimsDB = await userManager.GetClaimsAsync(user);
                claims.AddRange(claimsDB);

                return _jwtService.BuildToken(userCredentials,
                    claims,
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSetting:key"])),
                    DateTime.UtcNow.AddYears(1));
            }
            else
            {
                return BadRequest("incorrect login");
            }
        }
    }
}
