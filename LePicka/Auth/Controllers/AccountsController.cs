using Auth.AsyncDataServices;
using Auth.Data;
using Auth.Dto;
using Auth.Models;
using Auth.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
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
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public AccountsController(ILogger<AccountsController> logger, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IManageJWTService jwtService,
            IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            _jwtService = jwtService;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
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

                try
                {
                    var userCreatedDto = _mapper.Map<UserCreatedDto>(user);
                    userCreatedDto.Event = "User_Created";
                    userCreatedDto.DataSourceMicroserviceName = "Auth";
                    _messageBusClient.PublishUserCreatedEvent(userCreatedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"-->Could not send asynchronously {ex.Message}");
                }

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

        [HttpGet]
        public string Test()
        {
            _messageBusClient.PublishTestEvent();

            return "ok";
        }
    }
}
