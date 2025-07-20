using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations.admin
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Property>> GetAllWithHostAsync()
        {
            return await _context.Properties
                .Include(p => p.Host)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<Property> GetByIdWithHostAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Host)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Property>> GetByHostIdAsync(int hostId)
        {
            return await _context.Properties
                .Where(p => p.HostId == hostId)
                .Include(p => p.Host)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByStatusAsync(string status)
        {
            return await _context.Properties
                .Where(p => p.Status == status)
                .Include(p => p.Host)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, string status, string adminNotes = null)
        {
            var property = await GetByIdAsync(id);
            if (property != null)
            {
                property.Status = status;
                property.UpdatedAt = DateTime.UtcNow;
                Update(property);
            }
        }
    }
}
