using bnbClone_API.Data;
using bnbClone_API.DTOs.Auth;
using bnbClone_API.Models;
using bnbClone_API.Services;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailService emailService;
        private readonly IConfiguration config;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ITokenService tokenService,
            ILogger<AuthService> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
             IEmailService emailService,          // ✅ أضف هذا
             IConfiguration config
            )
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
            this._dbContext = dbContext;
            this.emailService = emailService;
            this.config = config;
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
                    var confirmationLink = $"{config["AppUrl"]}/api/auth/confirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

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

        //        // Update user role to host
        //        user.Role = UserRole.Host.ToString();
        //        user.UpdatedAt = DateTime.UtcNow;

        //        await _unitOfWork.Hosts.AddAsync(host);
        //        _unitOfWork.Users.Update(user);
        //        await _unitOfWork.SaveAsync();

        //        return new HostRegistrationResponseDto
        //        {
        //            HostId = host.Id,
        //            Message = "Successfully registered as a host",
        //            NewRole = UserRole.Host.ToString(),
        //            StartDate = host.StartDate
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during host registration");
        //        throw;
        //    }
        //}





        //public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshTokenDto.RefreshToken);
        //        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        //        {
        //            throw new UnauthorizedAccessException("Invalid refresh token");
        //        }

        //        // Generate new tokens
        //        var jwtToken = _tokenService.GenerateJwtToken(user);
        //        user.RefreshToken = _tokenService.GenerateRefreshToken();
        //        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        //        _unitOfWork.Users.Update(user);
        //        await _unitOfWork.SaveAsync();

        //        return new AuthResponseDto
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Role = user.Role,
        //            Token = jwtToken,
        //            RefreshToken = user.RefreshToken,
        //            TokenExpiry = DateTime.UtcNow.AddHours(1)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during token refresh");
        //        throw;
        //    }
        //}

        //public async Task<bool> RevokeTokenAsync(string refreshToken)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
        //        if (user == null) return false;

        //        user.RefreshToken = null;
        //        user.RefreshTokenExpiryTime = null;

        //        _unitOfWork.Users.Update(user);
        //        await _unitOfWork.SaveAsync();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during token revocation");
        //        return false;
        //    }
        //}

        //// NEW METHODS


        //public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetUserWithHostAsync(userId);
        //        if (user == null)
        //        {
        //            throw new InvalidOperationException("User not found");
        //        }

        //        var userProfile = new UserProfileDto
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            PhoneNumber = user.PhoneNumber,
        //            DateOfBirth = user.DateOfBirth,
        //            Gender = user.Gender,
        //            ProfilePictureUrl = user.ProfilePictureUrl,
        //            AccountStatus = user.AccountStatus,
        //            EmailVerified = user.EmailVerified,
        //            PhoneVerified = user.PhoneVerified,
        //            CreatedAt = user.CreatedAt,
        //            LastLogin = user.LastLogin,
        //            Role = user.Role
        //        };

        //        // Include host profile if user is a host
        //        if (user.Role == "host")
        //        {
        //            var host = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
        //            if (host != null)
        //            {
        //                userProfile.HostProfile = new HostProfileDto
        //                {
        //                    Id = host.Id,
        //                    StartDate = host.StartDate,
        //                    AboutMe = host.AboutMe,
        //                    Work = host.Work,
        //                    Rating = host.Rating,
        //                    TotalReviews = host.TotalReviews,
        //                    Education = host.Education,
        //                    Languages = host.Languages,
        //                    IsVerified = host.IsVerified,
        //                    TotalEarnings = host.TotalEarnings,
        //                    AvailableBalance = host.AvailableBalance,
        //                    LivesIn = host.LivesIn,
        //                    DreamDestination = host.DreamDestination,
        //                    FunFact = host.FunFact,
        //                    Pets = host.Pets,
        //                    ObsessedWith = host.ObsessedWith,
        //                    SpecialAbout = host.SpecialAbout
        //                };
        //            }
        //        }

        //        return userProfile;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting user profile");
        //        throw;
        //    }
        //}

        //public async Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateProfileDto updateProfileDto)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        //        if (user == null)
        //        {
        //            throw new InvalidOperationException("User not found");
        //        }

        //        // Update user fields
        //        if (!string.IsNullOrEmpty(updateProfileDto.FirstName))
        //            user.FirstName = updateProfileDto.FirstName;

        //        if (!string.IsNullOrEmpty(updateProfileDto.LastName))
        //            user.LastName = updateProfileDto.LastName;

        //        if (!string.IsNullOrEmpty(updateProfileDto.PhoneNumber))
        //            user.PhoneNumber = updateProfileDto.PhoneNumber;

        //        if (updateProfileDto.DateOfBirth.HasValue)
        //            user.DateOfBirth = updateProfileDto.DateOfBirth;

        //        if (!string.IsNullOrEmpty(updateProfileDto.Gender))
        //            user.Gender = updateProfileDto.Gender;

        //        if (!string.IsNullOrEmpty(updateProfileDto.ProfilePictureUrl))
        //            user.ProfilePictureUrl = updateProfileDto.ProfilePictureUrl;

        //        user.UpdatedAt = DateTime.UtcNow;

        //        // Update host profile if user is a host
        //        if (user.Role == "host")
        //        {
        //            var host = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
        //            if (host != null)
        //            {
        //                if (!string.IsNullOrEmpty(updateProfileDto.AboutMe))
        //                    host.AboutMe = updateProfileDto.AboutMe;

        //                if (!string.IsNullOrEmpty(updateProfileDto.Work))
        //                    host.Work = updateProfileDto.Work;

        //                if (!string.IsNullOrEmpty(updateProfileDto.Education))
        //                    host.Education = updateProfileDto.Education;

        //                if (!string.IsNullOrEmpty(updateProfileDto.Languages))
        //                    host.Languages = updateProfileDto.Languages;

        //                if (!string.IsNullOrEmpty(updateProfileDto.LivesIn))
        //                    host.LivesIn = updateProfileDto.LivesIn;

        //                if (!string.IsNullOrEmpty(updateProfileDto.DreamDestination))
        //                    host.DreamDestination = updateProfileDto.DreamDestination;

        //                if (!string.IsNullOrEmpty(updateProfileDto.FunFact))
        //                    host.FunFact = updateProfileDto.FunFact;

        //                if (!string.IsNullOrEmpty(updateProfileDto.Pets))
        //                    host.Pets = updateProfileDto.Pets;

        //                if (!string.IsNullOrEmpty(updateProfileDto.ObsessedWith))
        //                    host.ObsessedWith = updateProfileDto.ObsessedWith;

        //                if (!string.IsNullOrEmpty(updateProfileDto.SpecialAbout))
        //                    host.SpecialAbout = updateProfileDto.SpecialAbout;

        //                _unitOfWork.Hosts.Update(host);
        //            }
        //        }

        //        _unitOfWork.Users.Update(user);
        //        await _unitOfWork.SaveAsync();

        //        // Return updated profile
        //        return await GetUserProfileAsync(userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while updating user profile");
        //        throw;
        //    }
        //}

        //public async Task<bool> ConfirmEmailAsync(int userId, string token)
        //{
        //    // Implementation for email confirmation
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> SendPasswordResetAsync(string email)
        //{
        //    // Implementation for password reset
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        //{
        //    // Implementation for password reset
        //    throw new NotImplementedException();
        //}


    }
}