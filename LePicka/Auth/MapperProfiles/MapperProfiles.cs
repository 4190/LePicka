using Auth.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Auth.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            // Source -> Target
            CreateMap<IdentityUser, UserCreatedDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber : "-"));
        }
    }
}
