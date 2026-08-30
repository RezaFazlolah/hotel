using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class CreateHotelCommandValidator
    : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().
            WithMessage("Name is required");
        
        RuleFor(c => c.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(c => c.Rating)
            .ValidHotelRating();
    }
}