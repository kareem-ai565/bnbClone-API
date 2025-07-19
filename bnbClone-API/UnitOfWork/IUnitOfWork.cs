using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepo BookingRepo { get; }
        Task<int> SaveChanges();

    }
}
