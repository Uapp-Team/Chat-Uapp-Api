using AutoMapper;
using ChatUapp.Core.Accounts.DTOs;
using Volo.Abp.Identity;

namespace ChatUapp;

public class ChatUappApplicationAutoMapperProfile : Profile
{
    public ChatUappApplicationAutoMapperProfile()
    {
        CreateMap<IdentityUser, AppIdentityUserDto>()
        .IncludeBase<IdentityUser, IdentityUserDto>();
        CreateMap<IdentityUser, AppProfileDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
