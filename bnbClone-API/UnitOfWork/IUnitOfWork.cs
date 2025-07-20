using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

       
        IBookingRepo BookingRepo { get; }


        IAmenityRepo _Amenities { get; }
        IPropertyAmenityRepo PropAmenities { get; }
        IPropertyCategoryRepo PropCategory { get; }
        IBookingPaymentRepo BookingPaymentRepo { get; }
        IBookingPayoutRepo BookingPayoutRepo { get; }
        IHostPayoutRepo HostPayoutRepo { get; }
        IHostVerificationRepo hostVerification { get; }


        IUserRepository Users { get; }
        IHostRepository Hosts { get; } // Add this

        IPropertyRepository Properties { get; }
        IViolationRepository Violations { get; }
        IHostVerificationRepository HostVerifications { get; }
        INotificationRepository Notifications { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task <int>SaveAsync();

    }
}
