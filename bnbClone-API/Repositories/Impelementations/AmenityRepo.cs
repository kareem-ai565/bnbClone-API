using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.Repositories.Impelementations
{
    public class AmenityRepo : GenericRepo<Amenity> , IAmenityRepo
    {
        public AmenityRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
