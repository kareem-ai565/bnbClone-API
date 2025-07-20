using bnbClone_API.DTOs.AdminDTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListDto>> GetAllUsersAsync();
        Task<AdminUserResponseDto> GetUserByIdAsync(int userId);
        Task<bool> BanUserAsync(int userId, BanUserRequestDto request);
        Task<bool> UnbanUserAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
    }
}