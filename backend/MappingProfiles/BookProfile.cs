using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos.Book;
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
                .ForMember(des => des.ImagePath, opt => opt.MapFrom(src => Path.Combine("Images", "Books", src.CoverName)))
                .ForMember(des => des.currently_borrowed, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().currently_borrowed : false));

            CreateMap<Book, GetBookForLibrarianDto>()
                .ForMember(des => des.Category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(des => des.Author_name, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(des => des.ImagePath, opt => opt.MapFrom(src => Path.Combine("Images", "Books", src.CoverName)))
                .ForMember(des => des.currently_borrowed, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().currently_borrowed : false))
                .ForMember(des => des.BorrowDate, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().BorrowDate : (DateTime?)null))
                .ForMember(des => des.ReturnDate, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().ReturnDate : (DateTime?)null))
                .ForMember(des => des.DueDate, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().DueDate : (DateTime?)null))
                .ForMember(des => des.User, opt => opt.MapFrom(src => src.Borrowed.FirstOrDefault() != null ? src.Borrowed.FirstOrDefault().User : null));

            CreateMap<Borrowed, GetBorrowedBookForUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Book.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => Path.Combine("Images", "Books", src.Book.CoverName)));
        }
    }

        
}
