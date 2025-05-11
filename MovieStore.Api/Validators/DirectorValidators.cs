using FluentValidation;
using MovieStore.Api.DTOs.Director;

namespace MovieStore.Api.Validators
{
    public class CreateDirectorDtoValidator : AbstractValidator<CreateDirectorDto>
    {
        public CreateDirectorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class UpdateDirectorDtoValidator : AbstractValidator<UpdateDirectorDto>
    {
        public UpdateDirectorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
} 