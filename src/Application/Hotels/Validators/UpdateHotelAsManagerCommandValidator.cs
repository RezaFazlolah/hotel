using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class UpdateHotelAsManagerCommandValidator
    : AbstractValidator<UpdateHotelAsManagerCommand>
{
    public UpdateHotelAsManagerCommandValidator()
    {
        Include(new UpdateHotelBaseCommandValidator());
    }
}