using bnbClone_API.DTOs.ProfileDTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ProfileUserDto>> GetUserProfile(int userId)
        {
            try
            {
                var profile = await _profileService.GetUserProfileAsync(userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the user profile");
            }
        }

        [HttpGet("host/{userId}")]
        public async Task<ActionResult<ProfileHostDto>> GetHostProfile(int userId)
        {
            try
            {
                var profile = await _profileService.GetHostProfileAsync(userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the host profile");
            }
        }

        [HttpPost("upload-profile-picture")]
        public async Task<ActionResult<string>> UploadProfilePicture([FromForm] ProfileUploadPictureDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var imageUrl = await _profileService.UploadProfilePictureAsync(userId, dto.ProfilePicture);
                return Ok(new { imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading the profile picture");
            }
        }

        [HttpGet("editProfile")]
        public async Task<ActionResult<ProfileUserDto>> GetEditProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var profile = await _profileService.GetUserProfileAsync(userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the profile for editing");
            }
        }

        [HttpPut("editProfile")]
        public async Task<ActionResult<ProfileUserDto>> UpdateProfile([FromBody] ProfileUpdateUserDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var updatedProfile = await _profileService.UpdateUserProfileAsync(userId, dto);
                return Ok(updatedProfile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserID")?.Value;
            Console.WriteLine("UserID Claim: " + userIdClaim);


            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }
    }
}
