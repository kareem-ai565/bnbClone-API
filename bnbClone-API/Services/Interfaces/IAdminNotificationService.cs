using bnbClone_API.DTOs.Admin;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminNotificationService
    {
        Task<AdminNotificationResponseDto> SendNotificationAsync(AdminNotificationRequestDto request);
        Task<AdminNotificationResponseDto> SendNotificationToAllUsersAsync(string message);
        Task<AdminNotificationResponseDto> SendNotificationToSpecificUsersAsync(string message, List<int> userIds);
    }
}