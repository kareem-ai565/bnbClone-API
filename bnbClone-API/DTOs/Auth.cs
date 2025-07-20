using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiry { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
    //////=======================================================///
    /////new DTOs for AuthController

    namespace API.DTOs.Auth
    {
        public class UpdateProfileDto
        {
            [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
            public string? FirstName { get; set; }

            [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
            public string? LastName { get; set; }

            public DateTime? DateOfBirth { get; set; }

            [StringLength(10)]
            public string? Gender { get; set; }

            [Url(ErrorMessage = "Invalid URL format")]
            public string? ProfilePictureUrl { get; set; }
        }

        public class ChangePasswordDto
        {
            [Required(ErrorMessage = "Current password is required")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "New password is required")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirm password is required")]
            [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
            public string ConfirmNewPassword { get; set; }
        }

        public class EmailVerificationDto
        {
            [Required]
            public string Email { get; set; }
        }

        // If you don't have these, add them as well
        public class ForgotPasswordDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public class ValidateResetTokenDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Token { get; set; }
        }

        public class ResetPasswordDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Token { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string NewPassword { get; set; }
        }

        public class PasswordResetResponseDto
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        public class GoogleAuthRequest
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string GoogleId { get; set; }
        }

    }
}