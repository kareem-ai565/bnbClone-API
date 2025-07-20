using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IFavouriteRepo: IGenericRepo <Favourite>
    {
        Task<IEnumerable<Favourite>> GetFavouritesByUserIdAsync(int userId);
        Task<Favourite?> GetFavouriteByUserAndPropertyAsync(int userId, int propertyId);
        Task<bool> IsPropertyFavouritedByUserAsync(int userId, int propertyId);


    }
}
