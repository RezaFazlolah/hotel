using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class InsertHotelCommandValidator : AbstractValidator<InsertHotelCommand>
{
    public InsertHotelCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().
            WithMessage("Name is required");
        
        RuleFor(c => c.Address)
            .NotEmpty()
            .WithMessage("Address is required");
        
        RuleFor(c => c.Rating)
            .InclusiveBetween(0, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}