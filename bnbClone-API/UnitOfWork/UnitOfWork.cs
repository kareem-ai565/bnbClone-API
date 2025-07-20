using bnbClone_API.Data;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBookingRepo BookingRepo { get; }
        public IFavouriteRepo FavouriteRepo { get; }
        public IAvailabilityRepo AvailabilityRepo { get; }
        public IViolationRepo ViolationRepo { get; }
        public UnitOfWork(
            ApplicationDbContext context,
            IBookingRepo bookingRepo,
            IFavouriteRepo favouriteRepo,
            IAvailabilityRepo availabilityRepo,
            IViolationRepo violationRepo)
        {
            _context = context;
            BookingRepo = bookingRepo;
            FavouriteRepo = favouriteRepo;
            AvailabilityRepo = availabilityRepo;
            ViolationRepo = violationRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
