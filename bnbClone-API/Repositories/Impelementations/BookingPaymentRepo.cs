using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace bnbClone_API.Repositories.Impelementations
{
    public class BookingPaymentRepo : GenericRepo<BookingPayment>, IBookingPaymentRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingPaymentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookingPayment>> GetByBookingIdAsync(int bookingId)
        {
            return await _dbContext.BookingPayments
                .Where(bp => bp.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingPayment>> GetByGuestIdAsync(int guestId)
        {
            return await _dbContext.BookingPayments
                .Include(bp => bp.Booking)
                .Where(bp => bp.Booking.GuestId == guestId)
                .ToListAsync();
        }

        public async Task<BookingPayment> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _dbContext.BookingPayments
         .Include(p => p.Booking)
         .ThenInclude(b => b.Property) 
         .FirstOrDefaultAsync(p => p.TransactionId == paymentIntentId);
        }

        public async Task<IEnumerable<BookingPayment>> GetByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.BookingPayments.Include(p => p.Booking)
         .ThenInclude(b => b.Property).Where(pr => pr.Booking.PropertyId == propertyId).ToListAsync();
        }

        public async Task<IEnumerable<BookingPayment>> GetSearchPaymentsAsync(string status = null, DateTime? fromDate = null, DateTime? toDate = null, int? guestId = null, int? propertyId = null)
        {
            var query = _dbContext.BookingPayments
       .Include(p => p.Booking)
       .ThenInclude(b => b.Property)
       .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status.ToLower() == status.ToLower());

            if (fromDate.HasValue)
                query = query.Where(p => p.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(p => p.CreatedAt <= toDate.Value);

            if (guestId.HasValue)
                query = query.Where(p => p.Booking.GuestId == guestId.Value);

            if (propertyId.HasValue)
                query = query.Where(p => p.Booking.PropertyId == propertyId.Value);

            return await query.ToListAsync();
        }
    }
}
