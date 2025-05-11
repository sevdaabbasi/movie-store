using FluentValidation;
using MovieStore.Api.DTOs.Movie;

namespace MovieStore.Api.Validators
{
    public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Year)
                .NotEmpty()
                .GreaterThan(1900)
                .LessThanOrEqualTo(DateTime.Now.Year);

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.GenreId)
                .NotEmpty();

            RuleFor(x => x.DirectorId)
                .NotEmpty();

            RuleFor(x => x.ActorIds)
                .NotEmpty()
                .WithMessage("At least one actor must be specified.");
        }
    }

    public class UpdateMovieDtoValidator : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Year)
                .NotEmpty()
                .GreaterThan(1900)
                .LessThanOrEqualTo(DateTime.Now.Year);

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.GenreId)
                .NotEmpty();

            RuleFor(x => x.DirectorId)
                .NotEmpty();

            RuleFor(x => x.ActorIds)
                .NotEmpty()
                .WithMessage("At least one actor must be specified.");
        }
    }
} 