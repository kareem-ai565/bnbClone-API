using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IReviewRepo : IGenericRepo<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(int propertyId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
    }
}
