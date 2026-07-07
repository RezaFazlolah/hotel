using FluentValidation;

namespace Application.Hotels.Commands.Validators;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().
            WithMessage("Name is required");
        
        RuleFor(c => c.Address)
            .NotEmpty()
            .WithMessage("Address is required");
        
        RuleFor(c => c.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}