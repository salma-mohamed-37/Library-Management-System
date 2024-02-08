using backend.Dtos.AddDtos;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddAuthorDtoValidation : AbstractValidator<AddAuthorDto>
    {
        public AddAuthorDtoValidation() 
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
