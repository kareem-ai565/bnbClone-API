using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Implementations.admin
{
    public class ViolationRepository : GenericRepository<Violation>, IViolationRepository
    {
        public ViolationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Violation>> GetAllWithDetailsAsync()
        {
            return await _context.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedProperty)
                .Include(v => v.ReportedHost)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<Violation> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedProperty)
                .Include(v => v.ReportedHost)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Violation>> GetByStatusAsync(string status)
        {
            return await _context.Violations
                .Where(v => v.Status == status)
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedProperty)
                .Include(v => v.ReportedHost)
                    .ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, string status, string adminNotes)
        {
            var violation = await GetByIdAsync(id);
            if (violation != null)
            {
                violation.Status = status;
                violation.AdminNotes = adminNotes;
                violation.UpdatedAt = DateTime.UtcNow;

                if (status == ViolationStatus.Resolved.ToString())
                {
                    violation.ResolvedAt = DateTime.UtcNow;
                }

                Update(violation);
            }
        }
    }
}