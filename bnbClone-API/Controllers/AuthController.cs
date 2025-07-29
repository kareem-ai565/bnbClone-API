using Azure.Core;
using bnbClone_API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

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

        //[HttpPost("login")]
        //public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        //{

        //    bool found = await _authService.UserFound(loginDto.Email);
        //    if (found)
        //    {
        //        ApplicationUser user = await userManager.FindByEmailAsync(loginDto.Email);
        //        if (await _authService.LoginAsync(loginDto) != null)
        //        {
        //            var tokenClaims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Email,user.Email ),
        //                new Claim(ClaimTypes.Name ,user.UserName),
        //                new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
        //                new Claim(ClaimTypes.Role ,user.Role)
        //            };

        //            if (user.HostId > 0)
        //            {
        //                tokenClaims.Add(new Claim("HostId", user.HostId.ToString()));
        //            }

        //            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
        //            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);




        //            JwtSecurityToken token = new JwtSecurityToken(

        //                 issuer: configuration["JWT:Issuer"],
        //                 audience: configuration["JWT:Audience"],
        //                 claims: tokenClaims,
        //                 expires: DateTime.UtcNow.AddHours(24),
        //                 signingCredentials: signingCredentials

        //                 );

        //            string Token = new JwtSecurityTokenHandler().WriteToken(token);


        //            return Ok(new { message = Token });

        //        }

        //        return BadRequest("change pass");
        //    }

        //    return BadRequest("Enter Valid email or signup");

        //}
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            bool found = await _authService.UserFound(loginDto.Email);
            if (found)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(loginDto.Email);
                var loggedInUser = await _authService.LoginAsync(loginDto);

                if (loggedInUser != null)
                {
                    // Get roles from UserManager (Identity system) using UserRoleConstants
                    var userRoles = await userManager.GetRolesAsync(user);

                    var tokenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserID", user.Id.ToString()) // Add this for your controller extraction
            };

                    // Add multiple role claims using UserRoleConstants
                    foreach (var role in userRoles)
                    {
                        tokenClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Add HostId if user has Host role using UserRoleConstants
                    if (userRoles.Contains(UserRoleConstants.Host))
                    {
                        // You can either get it from the logged in user or fetch it directly
                        var loggedUser = await _authService.LoginAsync(loginDto);
                        if (loggedUser?.HostId > 0)
                        {
                            tokenClaims.Add(new Claim("HostId", loggedUser.HostId.ToString()));
                        }
                    }

                    SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        claims: tokenClaims,
                        expires: DateTime.UtcNow.AddHours(24),
                        signingCredentials: signingCredentials
                    );

                    string Token = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new
                    {
                        message = Token,
                        roles = userRoles, // Optional: return roles in response
                        user = new
                        {
                            id = user.Id,
                            email = user.Email,
                            username = user.UserName,
                            roles = userRoles
                        }
                    });

  //                  var cookieOptions = new CookieOptions
    //                {
         //               HttpOnly = false,       // Allow JS to read (for debugging)
           //             Secure = true,          // Keep true for HTTPS
             //           SameSite = SameSiteMode.None,
               //         Domain = "localhost",   // Critical for localhost
                 //       Path = "/",             // Explicit path
                   //     Expires = DateTimeOffset.UtcNow.AddDays(10),
                    //    IsEssential = true
                 //   };
//
                  //  Response.Cookies.Append("access_token", Token, cookieOptions);
//
                  //  Console.WriteLine($"Cookie set: {token.ToString().Substring(0, 10)}..."); 
                 //   Console.WriteLine($"Headers: {JsonSerializer.Serialize(Response.Headers)}");
//
//
//
  //                  return Ok(new { message = "Login successful" });
//
 //                  

                }
                return BadRequest("Invalid password");
            }
            return BadRequest("Enter valid email or signup");
        }

        //[HttpPost("register-host")]
        //[Authorize(Roles =UserRoleConstants.Guest)]
        //public async Task<ActionResult> RegisterHost([FromBody] RegisterHostDto registerHostDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState });

        //    try
        //    {
        //        // Extract UserID from JWT claims with multiple fallbacks
        //        var userIdClaim = User.FindFirst("UserID")?.Value ??
        //                         User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        //        {
        //            _logger.LogWarning("Invalid UserID claim. Value: {UserIdClaim}", userIdClaim ?? "NULL");
        //            return Unauthorized(new { success = false, message = "Invalid user token" });
        //        }

        //        ///// service to register the host
        //        var result = await _authService.RegisterHostAsync(userId, registerHostDto);

        //        return Ok(new
        //        {
        //            success = true,
        //            message = result.Message,
        //            hostId = result.HostId,
        //            newRole = result.NewRole,
        //            startDate = result.StartDate
        //        });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogWarning(ex, "Invalid operation during host registration");
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during host registration");
        //        return StatusCode(500, new { success = false, message = "An error occurred while processing your request" });
        //    }
        //}





//         [HttpPost("LogOut")]
//         public IActionResult LogOut()
//         {
//             Response.Cookies.Delete("access_token");
//             return Ok(new { message = "Logout Successfully" });

//         }
        
        
        
        
        
        [HttpPost("register-host")]
        [Authorize(Roles = UserRoleConstants.Guest)] // This will still work since users keep Guest role
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

                // Service to register the host
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