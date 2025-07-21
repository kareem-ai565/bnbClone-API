using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IPromotionRepository : IGenericRepo<Promotion>
    {
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
    }
}
