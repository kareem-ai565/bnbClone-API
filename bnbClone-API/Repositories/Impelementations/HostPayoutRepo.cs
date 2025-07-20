using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.Repositories.Impelementations
{
    public class HostPayoutRepo:GenericRepo<HostPayout>, IHostPayoutRepo
    {
        private readonly ApplicationDbContext dbContext;
        public HostPayoutRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
