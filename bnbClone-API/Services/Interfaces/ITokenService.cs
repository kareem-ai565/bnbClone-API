using bnbClone_API.Models;

namespace bnbClone_API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user);
        string GenerateRefreshToken();
        bool ValidateJwtToken(string token);
        int GetUserIdFromToken(string token);
    }
}
