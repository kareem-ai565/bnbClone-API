using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Implementations
{
    public class PropertyImageRepo : GenericRepo<PropertyImage>, IPropertyImageRepo
    {
        private readonly ApplicationDbContext _context;

        public PropertyImageRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyImages
                .Where(img => img.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}
