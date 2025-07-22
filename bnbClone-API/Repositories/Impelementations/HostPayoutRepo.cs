using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class HostPayoutRepo:GenericRepo<HostPayout>, IHostPayoutRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public HostPayoutRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<IEnumerable<HostPayout>> GetAllAsync()
        {
            return await _dbContext.HostPayouts
                .Include(hp => hp.Host)
                .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<HostPayout>> GetByHostIdAsync(int hostId)
        {
            return await _dbContext.HostPayouts
                .Where(hp => hp.HostId == hostId)
                .ToListAsync();
        }

        public override async Task<HostPayout> GetByIdAsync(int id)
        {
            return await _dbContext.HostPayouts
                .Include(hp => hp.Host)
                .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(hp => hp.Id == id);
        }
    }
}
