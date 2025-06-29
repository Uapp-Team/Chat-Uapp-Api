using AutoMapper;
using ChatUapp.Accounts.DTOs;
using ChatUapp.AppIdentity;
using Volo.Abp.Identity;

namespace ChatUapp;

public class ChatUappApplicationAutoMapperProfile : Profile
{
    public ChatUappApplicationAutoMapperProfile()
    {
        CreateMap<AppIdentityUser, AppIdentityUserDto>().IncludeAllDerived();
        CreateMap<IdentityUser, AppProfileDto>().IncludeAllDerived();
        CreateMap<IdentityUser, AppIdentityUser>().IncludeAllDerived();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
