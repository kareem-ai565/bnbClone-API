using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.ProfileDTOs
{
    public class ProfileUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string AccountStatus { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }
    }

    public class ProfileUpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }
        public string NewPassWord { get; set; }
        //public string Email { get; set; }

        public string? ProfilePictureUrl { get; set; }

        //[StringLength(10)]
        //public string? Gender { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }

    public class ProfileUploadPictureDto
    {
        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
}
