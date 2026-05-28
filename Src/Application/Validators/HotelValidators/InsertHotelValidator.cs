using Application.Commands.HotelCommands;
using FluentValidation;

namespace Application.Validators.HotelValidators;

public class InsertHotelValidator : AbstractValidator<InsertHotel>
{
    public InsertHotelValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(c => c.Rating).InclusiveBetween(0, 5).WithMessage("Rating must be between 1 and 5");
    }
}