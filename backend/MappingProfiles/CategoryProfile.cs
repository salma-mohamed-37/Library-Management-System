using AutoMapper;
using backend.Dtos.AddDtos;
using backend.Dtos.GetDtos;
using backend.Models;

namespace backend.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();
        }
       
    }
}
