using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepo BookingRepo { get; }
        IFavouriteRepo FavouriteRepo { get; }
        IAvailabilityRepo AvailabilityRepo { get; }
        IViolationRepo ViolationRepo { get; }
        Task<int> SaveChangesAsync();
    }
}
