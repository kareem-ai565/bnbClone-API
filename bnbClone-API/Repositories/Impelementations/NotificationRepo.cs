using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        private readonly ApplicationDbContext dbContext;
        public NotificationRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Notification>> GetForUserAsync(int userId)
        {
            return await dbContext.Notifications.Include(n=>n.User)
                .Include(n=>n.Sender)
                .Where(n=>n.UserId == userId)
                .OrderByDescending(n=>n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await dbContext.Notifications
             .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await dbContext.Notifications
                .Where(n=>n.UserId == userId && !n.IsRead)
                .ToListAsync();
            foreach (var notification in notifications) {
                notification.IsRead = true;
            }
            dbContext.Notifications.UpdateRange(notifications);
            return true;
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await dbContext.Notifications.FindAsync(notificationId);
            if(notification == null) return false;
            notification.IsRead = true;
            dbContext.Notifications.Update(notification);
            return true;
        }
    }
}
