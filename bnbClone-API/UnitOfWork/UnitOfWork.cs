using bnbClone_API.Data;
using bnbClone_API.Repositories.Implementations;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPropertyRepo PropertyRepo { get; }
        public IPropertyImageRepo PropertyImageRepo { get; }
        public ICancellationPolicyRepo CancellationPolicies { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IPropertyRepo propertyRepo,
            IPropertyImageRepo propertyImageRepo,
            ICancellationPolicyRepo cancellationPolicyRepo)
        {
            _context = context;
            PropertyRepo = propertyRepo;
            PropertyImageRepo = propertyImageRepo;
            CancellationPolicies = cancellationPolicyRepo;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
