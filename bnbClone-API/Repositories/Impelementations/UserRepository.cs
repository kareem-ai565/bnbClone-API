using bnbClone_API.Data;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bnbClone_API.Repositories.Impelementations
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            return await _dbSet.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserWithHostAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        private ApplicationDbContext _context => (ApplicationDbContext)base._context;

    }

}