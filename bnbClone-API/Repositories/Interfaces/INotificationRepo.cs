using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        Task<List<Notification>> GetForUserAsync(int userId);
        Task<int> GetUnreadCountAsync (int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);

    }
}
