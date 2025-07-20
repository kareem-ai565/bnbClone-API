using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class AmenityRepo : GenericRepo<Amenity> , IAmenityRepo
    {
        private readonly ApplicationDbContext dbContext;

        public AmenityRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }



         
        public async Task<bool> DeleteAsync(int id)
        {

            Amenity amenity = dbContext.Amenities.FirstOrDefault(p => p.Id == id);

            if (amenity != null)
            {
                dbContext.Amenities.Remove(amenity);

                return true;
            }

            else
            {
                return false;
            }

        }
    }
}
