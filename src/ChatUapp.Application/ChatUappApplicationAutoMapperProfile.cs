using AutoMapper;
using ChatUapp.Core.Accounts.DTOs;
using ChatUapp.Core.Accounts.Entitys;
using Volo.Abp.Identity;

namespace ChatUapp;

public class ChatUappApplicationAutoMapperProfile : Profile
{
    public ChatUappApplicationAutoMapperProfile()
    {
        CreateMap<AppIdentityUser, AppIdentityUserDto>()
        .IncludeBase<IdentityUser, IdentityUserDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
