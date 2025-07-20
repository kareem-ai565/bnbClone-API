using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces.admin
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
        Task BulkInsertAsync(IEnumerable<Notification> notifications);
    }
}
