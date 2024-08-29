using backend.Dtos.AddDtos;
using backend.Interfaces;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddAuthorDtoValidation : AbstractValidator<AddAuthorDto>
    {
        public AddAuthorDtoValidation(IAuthorRepository authorRepository) 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Author name is required.")

                .MustAsync(async (name, cancellationToken) =>
                !await authorRepository.IsNameExists(name, cancellationToken)
            ).WithMessage("This Author already exists.");
        }
    }
}
