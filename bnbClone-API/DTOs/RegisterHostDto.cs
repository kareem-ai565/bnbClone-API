using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.Auth
{
    public class RegisterHostDto
    {
        [Required(ErrorMessage = "About Me is required")]
        public string AboutMe { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Work { get; set; }

        [StringLength(100)]
        public string? Education { get; set; }

        [StringLength(200)]
        public string? Languages { get; set; }

        [StringLength(100)]
        public string? LivesIn { get; set; }

        [StringLength(100)]
        public string? DreamDestination { get; set; }

        [StringLength(200)]
        public string? FunFact { get; set; }

        [StringLength(100)]
        public string? Pets { get; set; }

        [StringLength(200)]
        public string? ObsessedWith { get; set; }

        [StringLength(300)]
        public string? SpecialAbout { get; set; }
    }
    public class HostRegistrationResponseDto
    {
        public int HostId { get; set; }
        public string Message { get; set; }
        public string NewRole { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class UpdateProfileDto
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [Url]
        public string? ProfilePictureUrl { get; set; }

        // Host-specific fields (only updated if user is a host)
        [StringLength(500)]
        public string? AboutMe { get; set; }

        [StringLength(100)]
        public string? Work { get; set; }

        [StringLength(100)]
        public string? Education { get; set; }

        [StringLength(200)]
        public string? Languages { get; set; }

        [StringLength(100)]
        public string? LivesIn { get; set; }

        [StringLength(100)]
        public string? DreamDestination { get; set; }

        [StringLength(200)]
        public string? FunFact { get; set; }

        [StringLength(100)]
        public string? Pets { get; set; }

        [StringLength(200)]
        public string? ObsessedWith { get; set; }

        [StringLength(300)]
        public string? SpecialAbout { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string AccountStatus { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }

        // Host information (if user is a host)
        public HostProfileDto? HostProfile { get; set; }
    }

    public class HostProfileDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string? AboutMe { get; set; }
        public string? Work { get; set; }
        public decimal Rating { get; set; }
        public int TotalReviews { get; set; }
        public string? Education { get; set; }
        public string? Languages { get; set; }
        public bool IsVerified { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal AvailableBalance { get; set; }
        public string? LivesIn { get; set; }
        public string? DreamDestination { get; set; }
        public string? FunFact { get; set; }
        public string? Pets { get; set; }
        public string? ObsessedWith { get; set; }
        public string? SpecialAbout { get; set; }
    }


}