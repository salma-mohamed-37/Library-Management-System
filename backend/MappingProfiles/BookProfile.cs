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
            CreateMap<AddBookDto, Book>()
                .ForMember(des => des.CoverName, opt => opt.MapFrom(src => $"{Guid.NewGuid()}_{src.CoverFile.FileName}"));
            CreateMap<Book, GetBookDto>()
                .ForMember(des => des.Category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(des => des.Author_name, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(des => des.ImagePath, opt => opt.MapFrom(src => Path.Combine("Images", "Books", src.CoverName)));
        }
    }
}
