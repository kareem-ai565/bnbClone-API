using bnbClone_API.DTOs.MessagesDTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<MessageDTO>> GetMessagesByConversationIdAsync(int conversationId);
        Task<MessageDTO> GetMessageByIdAsync(int messageId);
        Task<MessageDTO> GetLatestMessageAsync(int conversationId);

        Task<bool> SendMessageAsync(SendMessageDTO messageDTO);
        Task<bool> MarkMessageAsReadAsync(int messageId);   
    }
}
