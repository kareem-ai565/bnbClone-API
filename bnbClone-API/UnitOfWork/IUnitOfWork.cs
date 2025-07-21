using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepo BookingRepo { get; }
        IMessageRepo MessageRepo { get; }
        IConversationRepo ConversationRepo { get; }
        INotificationRepo NotificationRepo { get; }
        Task<int> SaveAsync();
        
    }
}
