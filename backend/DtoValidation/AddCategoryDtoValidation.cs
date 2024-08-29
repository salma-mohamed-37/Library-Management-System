using backend.Dtos.AddDtos;
using backend.Interfaces;
using backend.Repositories;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddCategoryDtoValidation : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidation(ICategoryRepository categoryRepository) 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required.")

                .MustAsync(async (name, cancellationToken) =>
                !await categoryRepository.IsNameExists(name, cancellationToken)
            ).WithMessage("This Category already exists.");
        }
    }
}
