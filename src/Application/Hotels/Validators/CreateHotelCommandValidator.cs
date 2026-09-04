using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class CreateHotelCommandValidator
    : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Rating)
            .ValidHotelRating();
    }
}