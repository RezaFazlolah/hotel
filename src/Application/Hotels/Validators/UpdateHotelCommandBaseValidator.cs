using Application.Hotels.Commands;
using FluentValidation;

namespace Application.Hotels.Validators;

public class UpdateHotelCommandBaseValidator
    : AbstractValidator<UpdateHotelCommandBase>
{
    public UpdateHotelCommandBaseValidator()
    {
    }
}