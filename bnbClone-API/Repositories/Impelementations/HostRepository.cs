using bnbClone_API.Data;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class HostRepository : GenericRepository<Models.Host>, IHostRepository
    {
        public HostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Models.Host> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task<Models.Host> GetHostWithDetailsAsync(int hostId)
        {
            return await _dbSet
                .Include(h => h.User)
                .Include(h => h.Properties)
                .Include(h => h.Verifications)
                .FirstOrDefaultAsync(h => h.Id == hostId);
        }

        public async Task<IEnumerable<Models.Host>> GetTopRatedHostsAsync(int count = 10)
        {
            return await _dbSet
                .Include(h => h.User)
                .Where(h => h.TotalReviews > 0)
                .OrderByDescending(h => h.Rating)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> IsUserAlreadyHostAsync(int userId)
        {
            return await _dbSet.AnyAsync(h => h.UserId == userId);
        }

        public async Task<Models.Host> GetHostByUserIdAsync(int userId)
        {
            return await _context.Hosts
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task<Models.Host> GetHostWithUserAsync(int hostId)
        {
            return await _context.Hosts
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.Id == hostId);
        }
        public async Task<Models.Host> GetHostWithPropertiesAsync(int hostId)
        {
            return await _context.Hosts
                .Include(h => h.User)
                .Include(h => h.Properties)
                .FirstOrDefaultAsync(h => h.Id == hostId);
        }

        public async Task<Models.Host?> GetHostWithUserByIdAsync(int id)
        {
            return await _context.Hosts.Include(h => h.User).FirstOrDefaultAsync(h => h.Id == id);
        }

        private ApplicationDbContext _context => (ApplicationDbContext)base._context;
    }
}
