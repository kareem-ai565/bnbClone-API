using Azure.Core;
using bnbClone_API.DTOs.Auth;
using bnbClone_API.DTOs.Auth.API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
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
        private readonly IEmailService emailService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration , IEmailService emailService)
        {
            _authService = authService;
            _logger = logger;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailService = emailService;
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
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserID", user.Id.ToString()) // Add this for your controller extraction
            };

                    // Add multiple role claims using UserRoleConstants
                    foreach (var role in userRoles)
                    {
                        tokenClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Add HostId if user has Host role using UserRoleConstants
                    if (userRoles.Contains(UserRoleConstants.Host) && loggedInUser.HostId>0)
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




        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Invalid User");


            var decodedToken = Uri.UnescapeDataString(token);
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded) return BadRequest("Email confirmation failed");




            user.EmailVerified = true;
            user.AccountStatus = "active";
            await userManager.UpdateAsync(user);

            return Ok("Email confirmed successfully!");
        }





        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var user = await userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
                return BadRequest("User not found");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // عمل رابط لإعادة التعيين
            var resetLink = $"{configuration["AppUrl"]}/ResetPassword?email={Uri.EscapeDataString(forgotPassword.Email)}&token={Uri.EscapeDataString(token)}";

            // إرسال الإيميل
            await emailService.SendEmailAsync(forgotPassword.Email, "Reset Password",
                $"Click <a href='{resetLink}'>here</a> to reset your password");

            return Ok(new {message= "Password reset link has been sent to your email." });
        }





        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("User not found");

            var decodedToken = Uri.UnescapeDataString(resetPasswordDto.Token);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);


            if (result.Succeeded)
                return Ok("Password has been reset successfully!");

            return BadRequest(result.Errors.Select(e => e.Description));
        }







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

        ////google 
        // Add these methods to your AuthController class

        //[HttpPost("google-auth")]
        //public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthRequest googleUser)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Google auth request received for email: {Email}", googleUser?.Email);

        //        if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
        //        {
        //            _logger.LogWarning("Invalid Google user data received");
        //            return BadRequest(new { success = false, message = "Invalid Google user data" });
        //        }

        //        // Optional: Verify the Google ID token here for additional security
        //        if (!string.IsNullOrEmpty(googleUser.IdToken))
        //        {
        //            try
        //            {
        //                // Add token verification logic here if needed
        //                _logger.LogInformation("Google ID token received for verification");
        //            }
        //            catch (Exception tokenEx)
        //            {
        //                _logger.LogWarning(tokenEx, "Google token verification failed, proceeding without verification");
        //                // Continue without token verification for now
        //            }
        //        }

        //        // Get or create user from Google information
        //        var user = await _authService.GetOrCreateGoogleUserAsync(
        //            googleUser.Email,
        //            googleUser.FirstName ?? "",
        //            googleUser.LastName ?? ""
        //        );

        //        _logger.LogInformation("User retrieved/created successfully for email: {Email}", user.Email);

        //        // Generate tokens for the user
        //        var tokenResponse = await _authService.CreateTokenResponseAsync(user);

        //        _logger.LogInformation("Token response created successfully for user: {UserId}", user.Id);

        //        // Get user roles for response
        //        var userRoles = await userManager.GetRolesAsync(user);

        //        var response = new GoogleLoginResponse
        //        {
        //            Success = true,
        //            Message = "Google authentication successful",
        //            Data = new GoogleLoginData
        //            {
        //                Token = tokenResponse.Token,
        //                RefreshToken = tokenResponse.RefreshToken,
        //                User = new GoogleUserData
        //                {
        //                    Id = user.Id,
        //                    Email = user.Email,
        //                    Username = user.UserName,
        //                    Roles = userRoles.ToArray()
        //                },
        //                Roles = userRoles.ToArray()
        //            }
        //        };

        //        return Ok(response);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogWarning(ex, "Invalid operation during Google authentication for email: {Email}", googleUser?.Email);
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error during Google authentication for email: {Email}", googleUser?.Email);
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            message = "An error occurred during Google authentication",
        //            details = ex.Message // Remove this in production
        //        });
        //    }
        //}

        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthRequest googleUser)
        {
            try
            {
                _logger.LogInformation("Google auth request received for email: {Email}", googleUser?.Email);

                if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
                {
                    return BadRequest(new { success = false, message = "Invalid Google user data" });
                }

                // Get or create user from Google information
                var user = await _authService.GetOrCreateGoogleUserAsync(
                    googleUser.Email,
                    googleUser.FirstName ?? "",
                    googleUser.LastName ?? ""
                );

                // Generate tokens for the user
                var tokenResponse = await _authService.CreateTokenResponseAsync(user);

                // Get user roles for response
                var userRoles = await userManager.GetRolesAsync(user);

                return Ok(new
                {
                    success = true,
                    message = "Google authentication successful",
                    data = new
                    {
                        token = tokenResponse.Token,
                        refreshToken = tokenResponse.RefreshToken,
                        user = new
                        {
                            id = user.Id,
                            email = user.Email,
                            username = user.UserName,
                            roles = userRoles
                        },
                        roles = userRoles
                    }
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error during Google auth: {Message}. Inner: {InnerException}",
                    dbEx.Message, dbEx.InnerException?.Message);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Database error occurred",
                    details = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during Google authentication for email: {Email}", googleUser?.Email);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication for email: {Email}. Full exception: {Exception}",
                    googleUser?.Email, ex.ToString());

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred during Google authentication",
                    details = ex.InnerException?.Message ?? ex.Message,
                    fullError = ex.ToString() // Remove this in production
                });
            }
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (!authenticateResult.Succeeded)
                {
                    _logger.LogWarning("Google authentication failed");
                    return BadRequest(new { success = false, message = "Google authentication failed" });
                }

                var claims = authenticateResult.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Email claim not found in Google authentication");
                    return BadRequest(new { success = false, message = "Email claim not found" });
                }

                // Get additional claims
                var firstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
                var lastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

                // Get or create user
                var user = await _authService.GetOrCreateGoogleUserAsync(email, firstName, lastName);

                // Generate tokens
                var tokenResponse = await _authService.CreateTokenResponseAsync(user);

                // Return tokens to the frontend (adjust URL as needed)
                var redirectUrl = $"http://localhost:4200/login?access_token={tokenResponse.Token}";

                if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    redirectUrl += $"&refresh_token={tokenResponse.RefreshToken}";
                }

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google callback");
                return Redirect("http://localhost:4200/login?error=authentication_failed");
            }
        }






    }
}