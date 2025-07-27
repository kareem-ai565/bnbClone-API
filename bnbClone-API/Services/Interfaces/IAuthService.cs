using bnbClone_API.DTOs.Auth;
using bnbClone_API.Models;
using static bnbClone_API.Services.Impelementations.AuthService;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response<ApplicationUser>> RegisterAsync(RegisterDto registerDto);
        Task<bool> UserFound(string email);
        Task<ApplicationUser?> LoginAsync(LoginDto loginDto);



        //Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        //Task<bool> RevokeTokenAsync(string refreshToken);
        //Task<bool> ConfirmEmailAsync(int userId, string token);
        //Task<bool> SendPasswordResetAsync(string email);
        //Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        //// New methods for the additional endpoints
        Task<HostRegistrationResponseDto> RegisterHostAsync(int userId, RegisterHostDto registerHostDto);
        //Task<UserProfileDto> GetUserProfileAsync(int userId);
        //Task<UserProfileDto> UpdateUserProfileAsync(int userId, UpdateProfileDto updateProfileDto);

    }
}
