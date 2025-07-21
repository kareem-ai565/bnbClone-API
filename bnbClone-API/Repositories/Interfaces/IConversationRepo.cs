using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IConversationRepo:IGenericRepo<Conversation>
    {
        Task<List<Conversation>> GetUserConversationsAsync(int userId);
        Task<Conversation?> GetByIdWithUsersAndPropertyAsync(int id);
        Task<Conversation?> GetExistingBetweenUsersAsync(int user1Id , int user2Id,int? propertyId);
    }
}
