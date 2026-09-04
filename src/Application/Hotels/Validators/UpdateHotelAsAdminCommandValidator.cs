using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class UpdateHotelAsAdminCommandValidator
    :AbstractValidator<UpdateHotelAsAdminCommand>
{
    public UpdateHotelAsAdminCommandValidator()
    {
        Include(new UpdateHotelBaseCommandValidator());
        
        RuleFor(x => x.Rating)
            .ValidHotelRating();
    }
}