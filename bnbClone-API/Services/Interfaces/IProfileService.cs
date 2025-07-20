using bnbClone_API.DTOs.ProfileDTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileUserDto> GetUserProfileAsync(int userId);
        Task<ProfileHostDto> GetHostProfileAsync(int userId);
        Task<ProfileUserDto> UpdateUserProfileAsync(int userId, ProfileUpdateUserDto dto);
        Task<ProfileHostDto> UpdateHostProfileAsync(int userId, ProfileUpdateHostDto dto);
        Task<string> UploadProfilePictureAsync(int userId, IFormFile file);
    }
}
