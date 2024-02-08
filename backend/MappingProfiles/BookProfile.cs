using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.MappingProfiles
{
    public class BookProfile :Profile
    {
        public BookProfile() 
        {
            CreateMap<AddBookDto, Book>();
            CreateMap<Book, GetBookDto>()
                .ForMember(des => des.Category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(des => des.Author_name, opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}
