using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.ConversationsDTOs;
using bnbClone_API.DTOs.MessagesDTOs;
using bnbClone_API.DTOs.NotificationsDTOs;
using bnbClone_API.Models;
using static bnbClone_API.DTOs.MessagesDTOs.SendMessageDTO;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MessageDTO, Message>();
        CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src =>
                src.Sender != null ? src.Sender.FirstName : null
            )); 
        CreateMap<SendMessageDTO, Message>().ForMember(dest => dest.SentAt, opt => opt.MapFrom(_ => DateTime.UtcNow));           
        CreateMap<Conversation, ConversationResponseDTO>()
        .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property != null ? src.Property.Title : null))
        .ForMember(dest => dest.User1Name, opt => opt.MapFrom(src => src.User1.UserName))
        .ForMember(dest => dest.User2Name, opt => opt.MapFrom(src => src.User2.UserName))
         .ForMember(dest => dest.Messages, opt => opt.MapFrom(src =>
                src.Messages.OrderBy(m => m.SentAt)
            )); 
        CreateMap<StartConversationDTO, Conversation>();
        CreateMap<CreateNotificationDTO, Notification>();
        CreateMap<BroadcastNotificationDTO, Notification>(); // used inside foreach per user

        CreateMap<Notification, NotificationResponseDTO>()
            .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src =>
                src.Sender != null ? src.Sender.UserName : "System"));
    }
}