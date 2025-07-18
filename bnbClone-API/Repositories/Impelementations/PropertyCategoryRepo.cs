using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.Repositories.Impelementations
{
    public class PropertyCategoryRepo: GenericRepo<PropertyCategory> , IPropertyCategoryRepo
    {

        public PropertyCategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
