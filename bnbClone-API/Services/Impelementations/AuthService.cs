using bnbClone_API.Data;
using bnbClone_API.DTOs.Auth;
using bnbClone_API.DTOs.Auth.API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
//<<<<<<< Kareem-x
        private readonly IConfiguration _configuration;


//=======
        private readonly IEmailService emailService;
   //     private readonly IConfiguration config;
//>>>>>>> master

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ITokenService tokenService,
            ILogger<AuthService> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
//<<<<<<< Kareem-x
            IConfiguration configuration,
//=======
             IEmailService emailService      // ✅ أضف هذا
     //        IConfiguration config
//>>>>>>> master
            )
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
            this._dbContext = dbContext;
//<<<<<<< Kareem-x
           this._configuration = configuration;
//=======
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
      //      this.config = config;
//>>>>>>> master
        }

        public async Task<bool> UserFound(string email)
        {
            ApplicationUser user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public class Response<T> where T : class
        {
            public bool IsSucceed { get; set; }
            public List<string>? Errors { get; set; }
            public T? Data { get; set; }
        }

        public async Task<Response<ApplicationUser>> RegisterAsync(RegisterDto registerDto)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender,
                UserName = $"{registerDto.FirstName}{registerDto.LastName}",
                AccountStatus = "suspended"
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // إضافة المستخدم إلى الدور الافتراضي (Guest)
                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Guest.ToString());

                if (addToRoleResult.Succeeded)
                {
                    // ✅ 1. توليد توكن تأكيد البريد
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // ✅ 2. إنشاء رابط التفعيل (Frontend URL أو API Endpoint)
                    var confirmationLink = $"{_configuration["AppUrl"]}/api/auth/confirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                    // ✅ 3. إرسال البريد الإلكتروني
                    await emailService.SendEmailAsync(user.Email, "Confirm Your Email",
                        $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>");

                    // ✅ 4. إرجاع نجاح مع رسالة تطلب التفعيل
                    return new Response<ApplicationUser>
                    {
                        IsSucceed = true,
                        Data = user,
                        //Message = "Registration successful! Please check your email to confirm your account."
                    };
                }
            }

            // جمع الأخطاء لو فشل التسجيل
            var errors = result.Errors.Select(e => e.Description).ToList();

            return new Response<ApplicationUser>
            {
                IsSucceed = false,
                Errors = errors
            };
        }

        //public async Task<ApplicationUser?> LoginAsync(LoginDto loginDto)
        //{

        //    ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);

        //    if (await _userManager.CheckPasswordAsync(user, loginDto.Password))
        //    {
        //        var role = await _userManager.GetRolesAsync(user);

        //        user.Role = role?.FirstOrDefault() ?? string.Empty;

        //        //get the host if if the role is host from the database

        //        int hostID = await _dbContext.Hosts
        //            .Where(x=>x.UserId == user.Id)
        //            .Select(x=>x.Id)
        //            .FirstOrDefaultAsync();

        //        if(hostID > 0)
        //        {
        //            user.HostId = hostID;
        //        }

        //        return user;
        //    }
        //    return null;
        //}
        public async Task<ApplicationUser?> LoginAsync(LoginDto loginDto)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Keep the Role property for backward compatibility if needed
                user.Role = roles?.FirstOrDefault() ?? string.Empty;

                //get the host id if the user has Host role from the database
                if (roles.Contains(UserRoleConstants.Host))
                {
                    var host = await _unitOfWork.Hosts.GetByUserIdAsync(user.Id);
                    if (host != null)
                    {
                        user.HostId = host.Id;
                    }
                }

                return user;
            }
            return null;
        }


        

        public async Task<HostRegistrationResponseDto> RegisterHostAsync(int userId, RegisterHostDto registerHostDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                // Check if user is already a host
                var existingHost = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
                if (existingHost != null)
                {
                    throw new InvalidOperationException("User is already registered as a host");
                }

                // Get user from UserManager to work with Identity
                var identityUser = await _userManager.FindByIdAsync(userId.ToString());
                if (identityUser == null)
                {
                    throw new InvalidOperationException("User not found in Identity system");
                }

                // Get current roles
                var currentRoles = await _userManager.GetRolesAsync(identityUser);

                // Check if user has only Guest role
                if (!currentRoles.Contains(UserRoleConstants.Guest))
                {
                    throw new InvalidOperationException("Only guests can register as hosts");
                }

                // Check if user already has Host role
                if (currentRoles.Contains(UserRoleConstants.Host))
                {
                    throw new InvalidOperationException("User is already registered as a host");
                }

                // Check if user has roles other than Guest
                if (currentRoles.Count > 1 || (currentRoles.Count == 1 && !currentRoles.Contains(UserRoleConstants.Guest)))
                {
                    throw new InvalidOperationException("Only users with Guest role can register as hosts");
                }

                // Add Host role while keeping Guest role
                var addRoleResult = await _userManager.AddToRoleAsync(identityUser, UserRoleConstants.Host);
                if (!addRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to add user to Host role: {errors}");
                }

                // Create host record
                var host = new Models.Host
                {
                    UserId = userId,
                    StartDate = DateTime.UtcNow,
                    AboutMe = registerHostDto.AboutMe,
                    Work = registerHostDto.Work,
                    Education = registerHostDto.Education,
                    Languages = registerHostDto.Languages,
                    LivesIn = registerHostDto.LivesIn,
                    DreamDestination = registerHostDto.DreamDestination,
                    FunFact = registerHostDto.FunFact,
                    Pets = registerHostDto.Pets,
                    ObsessedWith = registerHostDto.ObsessedWith,
                    SpecialAbout = registerHostDto.SpecialAbout,
                    Rating = 0,
                    TotalReviews = 0,
                    IsVerified = false,
                    TotalEarnings = 0,
                    AvailableBalance = 0
                };

                // Update user timestamp (don't change the Role property since we're keeping both roles)
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Hosts.AddAsync(host);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();

                return new HostRegistrationResponseDto
                {
                    HostId = host.Id,
                    Message = "Successfully registered as a host",
                    NewRole = $"{UserRoleConstants.Guest}, {UserRoleConstants.Host}", // Indicate both roles
                    StartDate = host.StartDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during host registration for userId: {UserId}", userId);
                throw;
            }
        }


        //// Add these methods to your AuthService class


        //public async Task<ApplicationUser> GetOrCreateGoogleUserAsync(string email, string firstName, string lastName)




        //public async Task<HostRegistrationResponseDto> RegisterHostAsync(int userId, RegisterHostDto registerHostDto)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        //        if (user == null)
        //        {
        //            throw new InvalidOperationException("User not found");
        //        }

        //        // Check if user is already a host
        //        var existingHost = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
        //        if (existingHost != null)
        //        {
        //            throw new InvalidOperationException("User is already registered as a host");
        //        }

        //        // Get user from UserManager to work with Identity
        //        var identityUser = await _userManager.FindByIdAsync(userId.ToString());
        //        if (identityUser == null)
        //        {
        //            throw new InvalidOperationException("User not found in Identity system");
        //        }

        //        // Get current roles
        //        var currentRoles = await _userManager.GetRolesAsync(identityUser);

        //        // Remove from current role (Guest)
        //        if (currentRoles.Any())
        //        {
        //            var removeResult = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
        //            if (!removeResult.Succeeded)
        //            {
        //                var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
        //                throw new InvalidOperationException($"Failed to remove user from current role: {errors}");
        //            }
        //        }

        //        // Add to Host role using your constants
        //        var addRoleResult = await _userManager.AddToRoleAsync(identityUser, UserRoleConstants.Host);
        //        if (!addRoleResult.Succeeded)
        //        {
        //            var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
        //            throw new InvalidOperationException($"Failed to add user to Host role: {errors}");
        //        }

        //        // Create host record
        //        var host = new Models.Host
        //        {
        //            UserId = userId,
        //            StartDate = DateTime.UtcNow,
        //            AboutMe = registerHostDto.AboutMe,
        //            Work = registerHostDto.Work,
        //            Education = registerHostDto.Education,
        //            Languages = registerHostDto.Languages,
        //            LivesIn = registerHostDto.LivesIn,
        //            DreamDestination = registerHostDto.DreamDestination,
        //            FunFact = registerHostDto.FunFact,
        //            Pets = registerHostDto.Pets,
        //            ObsessedWith = registerHostDto.ObsessedWith,
        //            SpecialAbout = registerHostDto.SpecialAbout,
        //            Rating = 0,
        //            TotalReviews = 0,
        //            IsVerified = false,
        //            TotalEarnings = 0,
        //            AvailableBalance = 0
        //        };

        //        // Update user role property and timestamp
        //        user.Role = UserRoleConstants.Host;
        //        user.UpdatedAt = DateTime.UtcNow;

        //        await _unitOfWork.Hosts.AddAsync(host);
        //        _unitOfWork.Users.Update(user);
        //        await _unitOfWork.SaveAsync();

        //        return new HostRegistrationResponseDto
        //        {
        //            HostId = host.Id,
        //            Message = "Successfully registered as a host",
        //            NewRole = UserRoleConstants.Host,
        //            StartDate = host.StartDate
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during host registration for userId: {UserId}", userId);
        //        throw;
        //    }
        //}







        //   public async Task<HostRegistrationResponseDto> RegisterHostAsync(int userId, RegisterHostDto registerHostDto)

        //{
        //    try
        //    {
        //        // Check if user already exists
        //        var existingUser = await _userManager.FindByEmailAsync(email);
        //        if (existingUser != null)
        //        {
        //            // Update last login if needed
        //            existingUser.LastLogin = DateTime.UtcNow;
        //            await _userManager.UpdateAsync(existingUser);
        //            return existingUser;
        //        }

        //        // Create new user for Google authentication
        //        var newUser = new ApplicationUser
        //        {
        //            Email = email,
        //            UserName = $"{firstName}{lastName}{new Random().Next(1000, 9999)}", // Ensure unique username
        //            FirstName = firstName,
        //            LastName = lastName,
        //            EmailConfirmed = true, // Google emails are already verified
        //            AccountStatus = "Active",
        //            EmailVerified = true,
        //            CreatedAt = DateTime.UtcNow,
        //            LastLogin = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow
        //        };

        //        // Create user without password (Google OAuth)
        //        var result = await _userManager.CreateAsync(newUser);

        //        if (result.Succeeded)
        //        {
        //            // Add to Guest role by default
        //            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, UserRoleConstants.Guest);

        //            if (addToRoleResult.Succeeded)
        //            {
        //                _logger.LogInformation("Google user created successfully: {Email}", email);
        //                return newUser;
        //            }
        //            else
        //            {
        //                // If role assignment fails, delete the user and throw exception
        //                await _userManager.DeleteAsync(newUser);
        //                var roleErrors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
        //                throw new InvalidOperationException($"Failed to assign role to Google user: {roleErrors}");
        //            }
        //        }
        //        else
        //        {
        //            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //            throw new InvalidOperationException($"Failed to create Google user: {errors}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating/retrieving Google user for email: {Email}", email);
        //        throw;
        //    }
        //}
        public async Task<ApplicationUser> GetOrCreateGoogleUserAsync(string email, string firstName, string lastName)
        {
            try
            {
                _logger.LogInformation("Processing Google user: {Email}", email);

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    _logger.LogInformation("Existing user found: {UserId}", existingUser.Id);
                    existingUser.LastLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(existingUser);
                    return existingUser;
                }

                _logger.LogInformation("Creating new Google user for email: {Email}", email);

                // Generate unique username
                string baseUsername = $"{firstName}{lastName}".Replace(" ", "");
                string username = baseUsername;
                int counter = 1;

                // Ensure username is unique
                while (await _userManager.FindByNameAsync(username) != null)
                {
                    username = $"{baseUsername}{counter}";
                    counter++;
                }

                _logger.LogInformation("Generated unique username: {Username}", username);

                // Create new user for Google authentication
                var newUser = new ApplicationUser
                {
                    Email = email,
                    UserName = username,
                    NormalizedUserName = username.ToUpper(),
                    NormalizedEmail = email.ToUpper(),
                    FirstName = firstName ?? "Unknown",
                    LastName = lastName ?? "",
                    EmailConfirmed = true,
                    AccountStatus = "Active",
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                _logger.LogInformation("Creating user with username: {Username}", newUser.UserName);

                // ✅ SOLUTION: Create user with a dummy password since PasswordHash cannot be NULL
                string dummyPassword = Guid.NewGuid().ToString(); // Generate a random password
                var result = await _userManager.CreateAsync(newUser, dummyPassword);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully, adding to Guest role");

                    // Reload the user to get the generated ID
                    var createdUser = await _userManager.FindByEmailAsync(email);

                    // Add to Guest role by default
                    var addToRoleResult = await _userManager.AddToRoleAsync(createdUser, UserRoleConstants.Guest);

                    if (addToRoleResult.Succeeded)
                    {
                        _logger.LogInformation("Google user created successfully: {Email} with ID: {UserId}", email, createdUser.Id);
                        return createdUser;
                    }
                    else
                    {
                        var roleErrors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to assign role to Google user: {Errors}", roleErrors);

                        // Clean up - delete the user if role assignment fails
                        await _userManager.DeleteAsync(createdUser);
                        throw new InvalidOperationException($"Failed to assign role to Google user: {roleErrors}");
                    }
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create Google user: {Errors}", errors);
                    throw new InvalidOperationException($"Failed to create Google user: {errors}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating/retrieving Google user for email: {Email}. Inner exception: {InnerException}",
                    email, ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        public async Task<AuthResponseDto> CreateTokenResponseAsync(ApplicationUser user)
        {
            try
            {
                // Get user roles
                var userRoles = await _userManager.GetRolesAsync(user);

                var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("UserID", user.Id.ToString())
        };

                // Add role claims
                foreach (var role in userRoles)
                {
                    tokenClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Add HostId if user has Host role
                if (userRoles.Contains(UserRoleConstants.Host))
                {
                    var host = await _unitOfWork.Hosts.GetByUserIdAsync(user.Id);
                    if (host != null)
                    {
                        tokenClaims.Add(new Claim("HostId", host.Id.ToString()));
                    }
                }

                // Get JWT configuration (you'll need to inject IConfiguration)
                var jwtSettings = _configuration.GetSection("JWT");
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: tokenClaims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: signingCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new AuthResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRoles.ToArray(),
                    Token = tokenString,
                    RefreshToken = "", // You can implement refresh token logic if needed
                    TokenExpiry = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating token response for user: {UserId}", user.Id);
                throw;
            }
        }

        // Add this method to your AuthController or AuthService
        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid Google token", ex);
            }
        }

    }
}