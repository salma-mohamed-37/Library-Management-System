using backend.Dtos.AddDtos;
using backend.Interfaces;
using FluentValidation;

namespace backend.DtoValidation
{
    public class AddBorrowDtoValidation : AbstractValidator<AddBorrowDto>
    {
        public AddBorrowDtoValidation(IUserRepository userRepository, IBookRepository bookRepository) 
        {
            RuleFor(x=>x.UserId).MustAsync(userRepository.IsExists).WithMessage("This user doesn't exists");

            RuleFor(x => x.BooksIds)
               .Must(b => b.Any()).WithMessage("Books can't be empty")
               .ForEach(bookId =>
               bookId.MustAsync(async(id, cancellationToken)=> await bookRepository.IsExists(id, cancellationToken))
                    .WithMessage("This book doesn't exists"));
;        }
    }
}
