using backend.Dtos.AddDtos;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddBookDtoValidation : AbstractValidator<AddBookDto>
    {
        public AddBookDtoValidation() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PublishDate).NotEmpty();
            RuleFor(x => x.CoverFile)
            .Must(IsValidImage).WithMessage("The cover must have one from the following extensions: jpg, jpeg, png");
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.AuthorId).NotEmpty();
        }

        private bool IsValidImage(IFormFile file)
        {
            if (file == null)
                return true;

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(fileExtension.ToLower());
        }
    }
}
