using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class UpdateHotelBaseCommandValidator
    : AbstractValidator<UpdateHotelBaseCommand>
{
    public UpdateHotelBaseCommandValidator()
    {
    }
}