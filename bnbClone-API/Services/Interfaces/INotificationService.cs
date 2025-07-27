using bnbClone_API.DTOs.NotificationsDTOs;
using bnbClone_API.Models;

namespace bnbClone_API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDTO>> GetUserNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> MarkAsReadAsync (int id);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<NotificationResponseDTO?> CreateNotificationAsync(CreateNotificationDTO dto);
        //Task<List<NotificationResponseDTO>> BroadcastNotificationAsync(BroadcastNotificationDTO dto);

        Task<List<Notification>> GetAdminNotifications();
    }
}
