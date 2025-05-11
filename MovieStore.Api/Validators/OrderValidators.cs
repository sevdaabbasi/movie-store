using FluentValidation;
using MovieStore.Api.DTOs.Order;

namespace MovieStore.Api.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty();
        }
    }
} 