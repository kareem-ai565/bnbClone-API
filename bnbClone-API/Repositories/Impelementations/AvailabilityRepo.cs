using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace bnbClone_API.Repositories.Impelementations
{
    public class AvailabilityRepo : GenericRepo<PropertyAvailability>, IAvailabilityRepo
    {
        private readonly ApplicationDbContext db;

        public AvailabilityRepo(ApplicationDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<IEnumerable<PropertyAvailability>> FindByPropertyIdAsync(int propertyId)
        {
            return await db.PropertyAvailabilities
                .Where(a => a.PropertyId == propertyId)
                .ToListAsync();
        }
        public override async Task<bool> DeleteAsync(int id) //to cancel soft delete, to avoid implementing it (changing in db, make an enum)
        {
            var availability = await db.PropertyAvailabilities.FindAsync(id);
            if (availability == null) return false;

            db.PropertyAvailabilities.Remove(availability);
            return true;
        }
        public async Task<IEnumerable<PropertyAvailability>> GetAvailabilityByHostIdAsync(int hostId)
        {
            return await db.PropertyAvailabilities
                .Include(a => a.Property)
                .Where(a => a.Property.HostId == hostId)
                .ToListAsync();
        }
        public async Task<PropertyAvailability?> FindByPropertyAndDateAsync(int propertyId, DateTime date)
        {
            return await db.PropertyAvailabilities
                .FirstOrDefaultAsync(a =>
                    a.PropertyId == propertyId &&
                    a.Date.Date == date.Date); // Normalize date
        }

    }

}
