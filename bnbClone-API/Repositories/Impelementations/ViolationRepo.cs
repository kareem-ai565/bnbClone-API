using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class ViolationRepo : GenericRepo<Violation>, IViolationRepo
    {
        private readonly ApplicationDbContext _context;

        public ViolationRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Violation>> GetViolationsByReporterAsync(int userId)
        {
            return await _context.Violations
                .Where(v => v.ReportedById == userId)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsByPropertyAsync(int propertyId)
        {
            return await _context.Violations
                .Where(v => v.ReportedPropertyId == propertyId)
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsByStatusAsync(string status)
        {
            return await _context.Violations
                .Where(v => v.Status.ToLower() == status.ToLower())
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedProperty)
                .Include(v => v.ReportedHost)
                .ToListAsync();
        }

        public async Task<IEnumerable<Violation>> GetViolationsWithDetailsAsync()
        {
            return await _context.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .ToListAsync();
        }

        public async Task<Violation?> GetViolationByIdWithDetailsAsync(int violationId)
        {
            return await _context.Violations
                .Include(v => v.ReportedBy)
                .Include(v => v.ReportedHost).ThenInclude(h => h.User)
                .Include(v => v.ReportedProperty).ThenInclude(p => p.PropertyImages)
                .FirstOrDefaultAsync(v => v.Id == violationId);
        }
    }

}
