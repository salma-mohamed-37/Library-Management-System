using backend.Dtos.AddDtos;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddCategoryDtoValidation : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidation() 
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
