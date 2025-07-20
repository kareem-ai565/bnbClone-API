using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace bnbClone_API.Repositories.Implementations
{
    public class FavouriteRepo : GenericRepo<Favourite>, IFavouriteRepo
    {
        private readonly ApplicationDbContext db;

        public FavouriteRepo(ApplicationDbContext context) : base(context)
        {
            db = context;
        }

        // Get all favourites for a user
        public async Task<IEnumerable<Favourite>> GetFavouritesByUserIdAsync(int userId)
        {
            return await db.Favourites
                .Where(f => f.UserId == userId)
                .Include(f => f.Property)
                .ThenInclude(p => p.PropertyImages)
                .ToListAsync();
        }

        // Get specific favourite by user and property
        public async Task<Favourite?> GetFavouriteByUserAndPropertyAsync(int userId, int propertyId)
        {
            return await db.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        // Check if property is favourited by user
        public async Task<bool> IsPropertyFavouritedByUserAsync(int userId, int propertyId)
        {
            return await db.Favourites
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }
        public override async Task<bool> DeleteAsync(int id) //to cancel soft delete, to avoid implementing it for favourites (changing in db, make an enum)
        {
            var favourite = await db.Favourites.FindAsync(id);
            if (favourite == null) return false;

            db.Favourites.Remove(favourite);
            return true;
        }

    }
}
