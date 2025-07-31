using AutoMapper;
using bnbClone_API.DTOs.ConversationsDTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;

namespace bnbClone_API.Services.Impelementations
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ConversationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public async Task<ConversationResponseDTO?> GetByIdWithUsersAndPropertyAsync(int Id)
        {
            var conversation = await unitOfWork.ConversationRepo.GetByIdWithUsersAndPropertyAsync(Id);
            return conversation == null ? null : mapper.Map<ConversationResponseDTO>(conversation);
        }

        public async Task<List<ConversationResponseDTO>> GetUserConversationsAsync(int userId)
        {
            var conversations = await unitOfWork.ConversationRepo.GetUserConversationsAsync(userId);
            return conversations == null ? null : mapper.Map<List<ConversationResponseDTO>>(conversations);
        }

        public async Task<int?> StartConversationAsync(StartConversationDTO startConversation)
        {
            var existing = await unitOfWork.ConversationRepo.GetExistingBetweenUsersAsync(
                startConversation.User1Id,
                startConversation.User2Id,
                startConversation.PropertyId
            );
            if (existing != null) return existing.Id;
            var conversation = mapper.Map<Conversation>(startConversation);
            conversation.CreatedAt = DateTime.UtcNow;
            await unitOfWork.ConversationRepo.AddAsync(conversation);
            var saved = await unitOfWork.SaveAsync();
            return saved > 0 ? conversation.Id : null;
        }

        public async Task<bool> UpdateConversationAsync(UpdateConversationDTO updateConversation)
        {
            var conversation = await unitOfWork.ConversationRepo.GetByIdAsync(updateConversation.ConversationId);
            if(conversation == null) return false;
            if (updateConversation.PropertyId.HasValue)
            {
                conversation.PropertyId = updateConversation.PropertyId;  
            }
            if (!string.IsNullOrEmpty(updateConversation.Subject))
            {
                conversation.Subject = updateConversation.Subject;
            }
            unitOfWork.ConversationRepo.UpdateAsync(conversation);
            var saved = await unitOfWork.SaveAsync();
            return saved > 0;
        }
    }
}
