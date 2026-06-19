using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Validators.HotelValidators;

public class UpdateHotelValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(c => c.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
    }
}