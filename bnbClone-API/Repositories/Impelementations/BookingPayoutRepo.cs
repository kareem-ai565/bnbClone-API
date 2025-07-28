using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

public class BookingPayoutRepo : GenericRepo<BookingPayout>, IBookingPayoutRepo
    {
    private readonly ApplicationDbContext dbContext;

    public BookingPayoutRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        this.dbContext = dbContext;
    }
    public override async Task<IEnumerable<BookingPayout>> GetAllAsync()
    {
        return await dbContext.BookingPayouts
         .AsNoTracking()
        .Include(bp => bp.Booking)
            .ThenInclude(b => b.Property)
                .ThenInclude(p => p.Host)
                    .ThenInclude(h => h.User) // Load Host's User info
        .Include(bp => bp.Booking)
            .ThenInclude(b => b.Guest)
        .ToListAsync();


    }

}
