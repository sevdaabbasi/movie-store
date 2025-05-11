using FluentValidation;
using MovieStore.Api.DTOs.Actor;

namespace MovieStore.Api.Validators
{
    public class CreateActorDtoValidator : AbstractValidator<CreateActorDto>
    {
        public CreateActorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class UpdateActorDtoValidator : AbstractValidator<UpdateActorDto>
    {
        public UpdateActorDtoValidator()
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