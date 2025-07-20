using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class PropertyCategoryRepo: GenericRepo<PropertyCategory> , IPropertyCategoryRepo
    {
        private readonly ApplicationDbContext dbContext;

        public PropertyCategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }



     

      
    }
}
