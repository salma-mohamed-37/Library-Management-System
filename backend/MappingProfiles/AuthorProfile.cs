using AutoMapper;
using backend.Dtos.Account;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.MappingProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile() 
        {
            CreateMap<AddAuthorDto,Author>();
            CreateMap<Author, GetAuthorDto>();

            CreateMap<ApplicationUser, UserDto>();
        }


    }
}
