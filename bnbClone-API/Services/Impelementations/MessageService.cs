using AutoMapper;
using bnbClone_API.DTOs.MessagesDTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using static bnbClone_API.DTOs.MessagesDTOs.MessageDTO;
using static bnbClone_API.DTOs.MessagesDTOs.SendMessageDTO;


namespace bnbClone_API.Services.Impelementations
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MessageService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<List<MessageDTO>> GetMessagesByConversationIdAsync(int ConversationId)
        {
            var messages = await unitOfWork.MessageRepo.GetMessagesByConversationIdAsync(ConversationId);
            var messagesDTO= mapper.Map<List<MessageDTO>>(messages);
            return messagesDTO;
        }
        public async Task<MessageDTO> GetMessageByIdAsync(int messageId)
        {
            var message = await unitOfWork.MessageRepo.GetMessageByIdAsync(messageId);
            var massgeDto = mapper.Map<MessageDTO>(message);
            return massgeDto;
        }
        public async Task<MessageDTO> GetLatestMessageAsync(int ConversationId)
        {
            var message = await unitOfWork.MessageRepo.GetLatestMessageAsync(ConversationId);
            return mapper.Map<MessageDTO>(message);
        }
        public async Task<bool> SendMessageAsync(SendMessageDTO messageDTO)
        {

            var message = mapper.Map<bnbClone_API.Models.Message>(messageDTO);
            await unitOfWork.MessageRepo.AddAsync(message);
            var result = await unitOfWork.SaveAsync();
            return result > 0;
           
        }
        public async Task<bool> MarkMessageAsReadAsync(int messageId)
        {
            var message = await unitOfWork.MessageRepo.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return false;
            }
            message.ReadAt= DateTime.UtcNow;
            unitOfWork.MessageRepo.UpdateAsync(message);
            var saved = await unitOfWork.SaveAsync();
            return saved > 0;

        }
    }
}
