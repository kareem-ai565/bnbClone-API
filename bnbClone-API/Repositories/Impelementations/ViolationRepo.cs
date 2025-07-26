using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace bnbClone_API.Repositories.Impelementations
{
    public class ViolationRepo : GenericRepo<Violation>, IViolationRepo
    {
        private readonly ApplicationDbContext db;

        public ViolationRepo(ApplicationDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<IEnumerable<Violation>> GetViolationsByReporterAsync(int userId)
        {
            return await db.Violations
                .Where(v => v.ReportedById == userId)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsByPropertyAsync(int propertyId)
        {
            return await db.Violations
                .Where(v => v.ReportedPropertyId == propertyId)
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsByStatusAsync(string status)
        {
            return await db.Violations
                .Where(v => v.Status.ToLower() == status.ToLower())
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedProperty)
                .Include(v => v.ReportedHost)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsWithDetailsAsync()
        {
            return await db.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .ToListAsync();
        }

        public async Task<Violation?> GetViolationByIdWithDetailsAsync(int violationId)
        {
            return await db.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .FirstOrDefaultAsync(v => v.Id == violationId);
        }
        public override async Task<bool> DeleteAsync(int id) //to cancel soft delete, to avoid implementing it for violations (changing in db, make an enum)
        {
            var violation = await db.Violations.FindAsync(id);
            if (violation == null) return false;

            db.Violations.Remove(violation);
            return true;
        }
        public async Task<IEnumerable<Violation>> GetViolationsByHostAsync(int hostId)
        {
            return await db.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost)
                    .ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty)
                    .ThenInclude(p => p.PropertyImages)
                .Where(v => v.ReportedHostId == hostId)
                .ToListAsync();
        }

    }

}
