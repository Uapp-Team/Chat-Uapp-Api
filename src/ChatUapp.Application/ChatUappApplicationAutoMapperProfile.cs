using AutoMapper;
using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs;
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
        CreateMap<Chatbot, ChatBotListDto>();
        CreateMap<Chatbot, ChatbotDto>()
        .ForMember(dest => dest.iconName, opt => opt.MapFrom(src => src.IconStyle.IconName))
        .ForMember(dest => dest.iconColor, opt => opt.MapFrom(src => src.IconStyle.IconColor));

        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
