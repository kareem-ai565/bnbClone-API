using bnbClone_API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthController(IAuthService authService, ILogger<AuthController> logger ,UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            this.userManager = userManager;
            this.configuration = configuration;
        }



        //Registeration

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if(registerDto == null)
            {
                return BadRequest(new { error = "Enter All Required Data" });
            }

            bool found =await _authService.UserFound(registerDto.Email);

            if (!found)
            {
                await  _authService.RegisterAsync(registerDto);
                return Ok(registerDto);
            }
            else
            {
                return BadRequest(new { error = "Change Email , U Register By it Before" });
            }





        }


        //login

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            
            bool found=  await _authService.UserFound(loginDto.Email);
            if (found)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(loginDto.Email);
                if (await _authService.LoginAsync(loginDto) != null)
                {
                   


                    var Tokenclaims = new []{
                        new Claim(ClaimTypes.Email,loginDto.Email ),
                        new Claim(ClaimTypes.Name ,user.FirstName),
                        new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
                        new Claim(ClaimTypes.Role ,user.Role)
                    };

                    SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    SigningCredentials signingCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);




                   JwtSecurityToken token = new JwtSecurityToken(  

                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        claims: Tokenclaims,
                        signingCredentials: signingCredentials

                        );

                 string Token=new JwtSecurityTokenHandler().WriteToken(token);


                    return Ok(new { message  = Token} );

                }

                return BadRequest("change pass");
            }

            return BadRequest("Enter Valid email or signup");

        }









        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        var result = await _authService.RefreshTokenAsync(refreshTokenDto);
        //        return Ok(new
        //        {
        //            success = true,
        //            message = "Token refreshed successfully",
        //            data = result
        //        });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { success = false, message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during token refresh");
        //        return StatusCode(500, new { success = false, message = "Internal server error" });
        //    }
        //}

        //[HttpPost("revoke-token")]
        //public async Task<ActionResult> RevokeToken([FromBody] string refreshToken)
        //{
        //    try
        //    {
        //        var result = await _authService.RevokeTokenAsync(refreshToken);
        //        if (result)
        //        {
        //            return Ok(new { success = true, message = "Token revoked successfully" });
        //        }

        //        return BadRequest(new { success = false, message = "Token revocation failed" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during token revocation");
        //        return StatusCode(500, new { success = false, message = "Internal server error" });
        //    }
        //}











        //if (!ModelState.IsValid)
        //    return BadRequest(ModelState);

        //try
        //{
        //    var result = await _authService.RegisterAsync(registerDto);
        //    return Ok(new
        //    {
        //        success = true,
        //        message = "User registered successfully",
        //        data = result
        //    });
        //}
        //catch (InvalidOperationException ex)
        //{
        //    return BadRequest(new { success = false, message = ex.Message });
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error occurred during registration");
        //    return StatusCode(500, new { success = false, message = "Internal server error" });
        //}



        // NEW ENDPOINTS

        // [HttpPost("register-host")]
        //// [Authorize] // User must be logged in to become a host
        // public async Task<ActionResult> RegisterHost([FromBody] RegisterHostDto registerHostDto)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        //         var result = await _authService.RegisterHostAsync(userId, registerHostDto);

        //         return Ok(new
        //         {
        //             success = true,
        //             message = "Successfully promoted to host",
        //             data = result
        //         });
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         return BadRequest(new { success = false, message = ex.Message });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred during host registration");
        //         return StatusCode(500, new { success = false, message = "Internal server error" });
        //     }
        // }

        // [HttpGet("profile")]
        //// [Authorize]
        // public async Task<ActionResult> GetProfile()
        // {
        //     try
        //     {
        //         var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        //         var profile = await _authService.GetUserProfileAsync(userId);

        //         return Ok(new
        //         {
        //             success = true,
        //             data = profile
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while getting user profile");
        //         return StatusCode(500, new { success = false, message = "Internal server error" });
        //     }
        // }

        // [HttpPut("profile")]
        //// [Authorize]
        // public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        //         var result = await _authService.UpdateUserProfileAsync(userId, updateProfileDto);

        //         return Ok(new
        //         {
        //             success = true,
        //             message = "Profile updated successfully",
        //             data = result
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while updating profile");
        //         return StatusCode(500, new { success = false, message = "Internal server error" });
        //     }
        // }

        // [HttpGet("me")]
        // //[Authorize] // Added authorization attribute
        // public ActionResult GetCurrentUser()
        // {
        //     try
        //     {
        //         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //         var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //         var firstName = User.FindFirst("firstName")?.Value;
        //         var lastName = User.FindFirst("lastName")?.Value;
        //         var role = User.FindFirst(ClaimTypes.Role)?.Value;

        //         return Ok(new
        //         {
        //             success = true,
        //             data = new
        //             {
        //                 id = userId,
        //                 email,
        //                 firstName,
        //                 lastName,
        //                 role
        //             }
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while getting current user");
        //         return StatusCode(500, new { success = false, message = "Internal server error" });
        //     }
        // }



    }
}