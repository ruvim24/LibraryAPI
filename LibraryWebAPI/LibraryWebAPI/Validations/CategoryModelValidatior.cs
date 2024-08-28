using FluentValidation;
using LibraryWebAPI.Dtos.Category;

namespace LibraryWebAPI.Validations
{
    public class CategoryModelValidatior : AbstractValidator<CreateCategoryDto>
    {
        public CategoryModelValidatior()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");
        }
    }
}
