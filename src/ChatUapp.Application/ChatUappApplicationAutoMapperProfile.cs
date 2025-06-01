using AutoMapper;
using ChatUapp.Accounts.DTOs;
using ChatUapp.AppIdentity;

namespace ChatUapp;

public class ChatUappApplicationAutoMapperProfile : Profile
{
    public ChatUappApplicationAutoMapperProfile()
    {
        CreateMap<AppIdentityUser, MyIdentityUserDto>().IncludeAllDerived();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
