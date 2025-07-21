using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public IMessageRepo MessageRepo {  get;  }
        public IBookingRepo BookingRepo { get; }

        public IConversationRepo ConversationRepo { get; }
        public INotificationRepo NotificationRepo { get; }

        public UnitOfWork(ApplicationDbContext dbContext , IMessageRepo messageRepo, IBookingRepo bookingRepo, IConversationRepo conversationRepo, INotificationRepo notificationRepo)
        {
            this.dbContext = dbContext;
            this.MessageRepo = messageRepo;
            this.BookingRepo = bookingRepo;
            ConversationRepo = conversationRepo;
            NotificationRepo = notificationRepo;    
        }
        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
        public void Dispose() {
            dbContext.Dispose();
        }
    }
}
