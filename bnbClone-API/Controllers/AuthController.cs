using Azure.Core;
using bnbClone_API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        /*
            1-default admin when the project run create it in the database like the migration
         */

        //service => call endpoints => addpend for the jwt token in the header auth
        //Registeration

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest(new { error = "Enter All Required Data" });
            }

            bool isUserExsits = await _authService.UserFound(registerDto.Email);


            if (isUserExsits)
                return BadRequest(new { error = "Change Email , U Register By it Before" });



            var registrationResult = await _authService.RegisterAsync(registerDto);
            return registrationResult.IsSucceed ? Ok(registrationResult.Data) : Ok(registrationResult.Errors);
        }


        //login

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {

            bool found = await _authService.UserFound(loginDto.Email);
            if (found)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(loginDto.Email);
                if (await _authService.LoginAsync(loginDto) != null)
                {
                    var Tokenclaims = new[]{
                        new Claim("Email",user.Email ),
                        new Claim("UserName" ,user.UserName),
                        new Claim("Role" ,user.Role),
                        new Claim("UserID" , user.Id.ToString())
                    };

                    SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);




                    JwtSecurityToken token = new JwtSecurityToken(

                         issuer: configuration["JWT:Issuer"],
                         audience: configuration["JWT:Audience"],
                         claims: Tokenclaims,
                         expires: DateTime.UtcNow.AddHours(24),
                         signingCredentials: signingCredentials

                         );

                    string Token = new JwtSecurityTokenHandler().WriteToken(token);

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, 
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(10)
                    };


                    Response.Cookies.Append("access_token", Token, cookieOptions);

                    return Ok(new { message = "Login successful" });

                   

                }

                return BadRequest("change pass");
            }

            return BadRequest("Enter Valid email or signup");

        }




        [HttpPost("LogOut")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logout Successfully" });

        }
        
        
        
        
        
        [HttpPost("register-host")]
        [Authorize]
        public async Task<ActionResult> RegisterHost([FromBody] RegisterHostDto registerHostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState });

            try
            {
                // Extract UserID from JWT claims with multiple fallbacks
                var userIdClaim = User.FindFirst("UserID")?.Value ??
                                 User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    _logger.LogWarning("Invalid UserID claim. Value: {UserIdClaim}", userIdClaim ?? "NULL");
                    return Unauthorized(new { success = false, message = "Invalid user token" });
                }

                // Call the service to register the host
                var result = await _authService.RegisterHostAsync(userId, registerHostDto);

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    hostId = result.HostId,
                    newRole = result.NewRole,
                    startDate = result.StartDate
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during host registration");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during host registration");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request" });
            }
        }
    }
}