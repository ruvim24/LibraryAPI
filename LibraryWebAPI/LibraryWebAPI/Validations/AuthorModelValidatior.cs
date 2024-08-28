using FluentValidation;
using LibraryWebAPI.Dtos.Author;

namespace LibraryWebAPI.Validations
{
    public class AuthorModelValidatior : AbstractValidator<CreateAuthorDto>
    {
        public AuthorModelValidatior()
        {
            RuleFor(x => x.AuthorName)
                .NotEmpty()
                .WithMessage("Name is required")
                /*.NotNull()*/
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");

        }
    }
}
