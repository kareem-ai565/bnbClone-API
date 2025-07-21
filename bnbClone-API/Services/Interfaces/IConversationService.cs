using bnbClone_API.DTOs.ConversationsDTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IConversationService
    {
        Task<List<ConversationResponseDTO>> GetUserConversationsAsync(int userId);
        Task<ConversationResponseDTO?> GetByIdWithUsersAndPropertyAsync(int Id);
        Task<int?> StartConversationAsync(StartConversationDTO startConversation);
        Task<bool> UpdateConversationAsync(UpdateConversationDTO updateConversation);
    }
}
