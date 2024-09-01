using backend.Dtos.Account;
using backend.Dtos.AddDtos;
using backend.Interfaces;
using FluentValidation;

namespace backend.DtoValidation
{
    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation(IUserRepository userRepository) 
        {
            RuleFor(x => x.Email).MustAsync(async (email, cancellationToken) =>
            !await userRepository.IsEmailExists(email,cancellationToken));

            RuleFor(x => x.Username).MustAsync(async (username, cancellationToken) =>
            !await userRepository.IsUsernameExists(username, cancellationToken));



        }
    }
}
