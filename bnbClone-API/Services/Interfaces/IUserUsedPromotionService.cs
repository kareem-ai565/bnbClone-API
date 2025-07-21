using bnbClone_API.DTOs;
using bnbClone_API.Models;

namespace bnbClone_API.Services.Interfaces
{
    public interface IUserUsedPromotionService
    {
        Task<IEnumerable<UserUsedPromotionDTO>> GetAllUserPromotions();
        Task<UserUsedPromotion> AddUserPromotion(UserUsedPromotionDTO usedPromotionDTO);
        Task<IEnumerable<UserUsedPromotionDTO>> GetAllPromotionsOfUser(int id);
    }
}
