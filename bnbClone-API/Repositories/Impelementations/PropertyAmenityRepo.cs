using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class PropertyAmenityRepo : GenericRepo<PropertyAmenity> , IPropertyAmenityRepo
    {
        private readonly ApplicationDbContext dbContext;

        public PropertyAmenityRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PropertyAmenity> DeleteAsync(int propId, int AmentId)
        {
          PropertyAmenity property=await  dbContext.PropertyAmenities.Where(x => x.AmenityId == AmentId && x.PropertyId == propId).FirstOrDefaultAsync();
           
                 dbContext.PropertyAmenities.Remove(property);
                 return property;
               
        }
    }
}
