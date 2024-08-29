using backend.Dtos.AddDtos;
using backend.Interfaces;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddBookDtoValidation : AbstractValidator<AddBookDto>
    {
        public AddBookDtoValidation(IBookRepository bookRepository, ICategoryRepository categoryRepository, IAuthorRepository authorRepository) 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Book name is required.")
                .MustAsync(async (name, cancellationToken) =>
                !await bookRepository.IsNameExists(name, cancellationToken)
            ).WithMessage("This book already exists.");


            RuleFor(x => x.PublishDate).NotEmpty().WithMessage("Publish date is required.")
                .Must(IsDateInPast).WithMessage("Publish date must be in the past.");

            RuleFor(x => x.CoverFile)
            .Must(IsValidImage).WithMessage("The cover must have one from the following extensions: jpg, jpeg, png");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category Id is required.")
                .MustAsync(categoryRepository.IsExists).WithMessage("This category doesn't exists.");

            RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Author Id is required.")
                .MustAsync(authorRepository.IsExists).WithMessage("This author doesn't exists.");



        }

        private bool IsValidImage(IFormFile file)
        {
            if (file == null)
                return true;

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(fileExtension.ToLower());
        }

        public bool IsDateInPast(DateTime date)
        {
            DateTime currentDateTime = DateTime.Now;

            if(date < currentDateTime)
                return true;
            return false;

        }
    }
}
