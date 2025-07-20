using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;

namespace bnbClone_API.Infrastructure

{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IHostRepository Hosts { get; } // Add this

        IPropertyRepository Properties { get; }
        IViolationRepository Violations { get; }
        IHostVerificationRepository HostVerifications { get; }
        INotificationRepository Notifications { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
