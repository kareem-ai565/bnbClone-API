using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations.admin
{
    public class HostVerificationRepository : GenericRepository<HostVerification>, IHostVerificationRepository
    {
        public HostVerificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HostVerification>> GetAllWithHostAsync()
        {
            return await _context.HostVerifications
                .Include(hv => hv.Host)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<HostVerification> GetByIdWithHostAsync(int id)
        {
            return await _context.HostVerifications
                .Include(hv => hv.Host)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(hv => hv.Id == id);
        }

        public async Task<IEnumerable<HostVerification>> GetByStatusAsync(string status)
        {
            return await _context.HostVerifications
                .Where(hv => hv.Status == status)
                .Include(hv => hv.Host)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, string status, string adminNotes = null)
        {
            var verification = await GetByIdAsync(id);
            if (verification != null)
            {
                verification.Status = status;

                if (status == "approved")
                {
                    verification.VerifiedAt = DateTime.UtcNow;
                    // Update host verification status
                    var host = await _context.Hosts.FindAsync(verification.HostId);
                    if (host != null)
                    {
                        host.IsVerified = true;
                    }
                }

                Update(verification);
            }
        }
    }
}