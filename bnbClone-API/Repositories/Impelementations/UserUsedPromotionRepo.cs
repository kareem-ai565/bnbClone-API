using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.Repositories.Impelementations
{
    public class UserUsedPromotionRepo:GenericRepo<UserUsedPromotion> , IUserUsedPromotionRepo
    {
        public UserUsedPromotionRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
