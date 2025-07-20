using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Impelementations.admin;
using bnbClone_API.Repositories.Implementations.admin;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;
using Microsoft.EntityFrameworkCore.Storage;

namespace bnbClone_API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Hosts = new HostRepository(_context); // Add this
            Properties = new PropertyRepository(_context);
            Violations = new ViolationRepository(_context);
            HostVerifications = new HostVerificationRepository(_context);
            Notifications = new NotificationRepository(_context);

        }

        public IUserRepository Users { get; private set; }
        public IHostRepository Hosts { get; private set; } // Add this
        public IPropertyRepository Properties { get; private set; }
        public IViolationRepository Violations { get; private set; }
        public IHostVerificationRepository HostVerifications { get; private set; }
        public INotificationRepository Notifications { get; private set; }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}