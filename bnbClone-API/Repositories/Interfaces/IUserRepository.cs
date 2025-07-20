using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<ApplicationUser> GetUserWithHostAsync(int userId); // Add this method

    }
}