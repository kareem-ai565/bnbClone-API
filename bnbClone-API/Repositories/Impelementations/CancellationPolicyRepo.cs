using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace bnbClone_API.Repositories.Impelementations
{
    public class CancellationPolicyRepo : GenericRepo<CancellationPolicy>, ICancellationPolicyRepo
    {
        private readonly ApplicationDbContext _context;

        public CancellationPolicyRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CancellationPolicy>> GetAllAsync()
        {
            return await _context.CancellationPolicies.ToListAsync();
        }

        public async Task<CancellationPolicy?> GetByIdAsync(int id)
        {
            return await _context.CancellationPolicies.FindAsync(id);
        }
    }

}
