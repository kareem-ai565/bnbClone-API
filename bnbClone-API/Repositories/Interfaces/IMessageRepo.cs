using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IMessageRepo :IGenericRepo<Message>
    {
        Task<List<Message>> GetMessagesByConversationIdAsync(int ConversationId);
        Task<Message?> GetMessageByIdAsync(int messageId);
        Task<Message?> GetLatestMessageAsync(int ConversationId);
    }
}
