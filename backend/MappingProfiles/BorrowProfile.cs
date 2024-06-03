using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Models;
using Microsoft.Build.Framework;

namespace backend.MappingProfiles
{
    public class BorrowProfile : Profile
    {
        public BorrowProfile() 
        {
            CreateMap<AddBorrowDto, Borrowed>()
                .ForMember(dest => dest.currently_borrowed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src=> DateTime.Now.AddDays(14)))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => new DateTime(9999, 1, 1)));

            CreateMap<Borrowed, GetBorrowerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName));
        }


    }
}
