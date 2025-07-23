using AutoMapper;
using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs;
using ChatUapp.Core.ChatbotManagement.DTOs.Session;
using ChatUapp.Core.ChatbotManagement.Entities;
using Volo.Abp.Identity;

namespace ChatUapp;

public class ChatUappApplicationAutoMapperProfile : Profile
{
    public ChatUappApplicationAutoMapperProfile()
    {
        CreateMap<IdentityUser, AppIdentityUserDto>()
        .IncludeBase<IdentityUser, IdentityUserDto>();
        CreateMap<IdentityUser, AppProfileDto>();
        CreateMap<Chatbot, CreateChatbotDto>();
        CreateMap<Chatbot, UpdateChatbotDto>();
        CreateMap<ChatSession, ChatSessionTitleDto>();
        CreateMap<Chatbot, ChatbotDto>()
        .ForMember(dest => dest.iconName, opt => opt.MapFrom(src => src.IconStyle.IconName))
        .ForMember(dest => dest.iconColor, opt => opt.MapFrom(src => src.IconStyle.IconColor));
        CreateMap<Chatbot, ChatBotListDto>()
        .ForMember(dest => dest.iconName, opt => opt.MapFrom(src => src.IconStyle.IconName))
        .ForMember(dest => dest.iconColor, opt => opt.MapFrom(src => src.IconStyle.IconColor));
        CreateMap<ChatSession, ChatSessionDto>();
        CreateMap<ChatMessage, ChatMessageDto>()
        .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Value))
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Value));


        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
