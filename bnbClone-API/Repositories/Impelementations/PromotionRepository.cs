using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace bnbClone_API.Repositories.Impelementations
{
    public class PromotionRepository : GenericRepo<Promotion>, IPromotionRepository
    {
        private readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            return await _context.Promotions
                .Where(p => p.IsActive && p.StartDate <= DateTime.UtcNow && p.EndDate >= DateTime.UtcNow)
                .ToListAsync();
        }
    }

}
