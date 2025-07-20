using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.Repositories.Impelementations
{
    public class HostVerificationRepo:GenericRepo<HostVerification> , IHostVerificationRepo
    {

        public HostVerificationRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


    }
}
