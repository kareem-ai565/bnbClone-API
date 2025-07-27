using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace bnbClone_API.Repositories.Impelementations
{
    public class BookingRepo : GenericRepo<Booking>, IBookingRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        

        public override async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _dbContext.Bookings.Include(b=>b.Guest).Include(b => b.Property).ToListAsync();
        }

        public async Task<Booking> GetGuestByBookingIdAsync(int bookingId)
        {
            return await _dbContext.Bookings
        .Include(b => b.Guest)
        .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetByGuestIdAsync(int id)
        {
            return await _dbContext.Bookings
                .AsNoTracking()
                .Include(b => b.Guest)
                .Include(b => b.Property)
                .Where(b => b.GuestId == id)
                .ToListAsync();
        }

        public override async Task<Booking> GetByIdAsync(int id)
        {
            return await _dbContext.Bookings.Include(b=>b.Guest)
                .Include(b=>b.Property)
                .FirstOrDefaultAsync(b=>b.Id==id);
        }

        public async Task<IEnumerable<Booking>> GetByPropertyIdAsync(int id)
        {
            return await _dbContext.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Property)
                .Where(b => b.PropertyId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetSearchBookingAsync(string status = null, DateTime? fromDate = null, DateTime? toDate = null, int? guestId = null, int? propertyId = null)
        {
            var query = _dbContext.Bookings.Include(b => b.Guest).Include(b => b.Property).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                var normalizedStatus = char.ToUpper(status[0]) + status.Substring(1).ToLower();

                query = query.Where(b => b.Status == normalizedStatus);
            }

            if (fromDate.HasValue)
                query = query.Where(b => b.StartDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(b => b.EndDate <= toDate.Value);

            if (guestId.HasValue)
                query = query.Where(b => b.GuestId == guestId.Value);

            if (propertyId.HasValue)
                query = query.Where(b => b.PropertyId == propertyId.Value);

            return await query.ToListAsync();

        }
        public async Task<IEnumerable<Booking>> GetByHostIdAsync(int hostId)
        {
            return await _dbContext.Bookings
                .Include(b => b.Property)
                .Include(b => b.Guest)
                .Where(b => b.Property.HostId == hostId)
                .ToListAsync();
        }


    }
}
