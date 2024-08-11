using AutoMapper;
using backend.Dtos.Account;
using backend.Models;

namespace backend.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<ApplicationUser, UserDto>()
            .ForMember(des => des.ImagePath, opt => opt.MapFrom(src => Path.Combine("StaticFiles", "Images", "Users", src.ImagePath)));

        }
    }
}
