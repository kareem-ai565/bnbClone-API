using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int propertyId)
        {
            return await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Booking)
                .Where(r => r.Booking.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Booking)
                .Include(r => r.Reviewer)
                .Where(r => r.ReviewerId == userId)
                .ToListAsync();
        }

    }
}
