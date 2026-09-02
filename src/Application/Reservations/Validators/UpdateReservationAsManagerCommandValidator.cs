using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationAsManagerCommandValidator
    :AbstractValidator<UpdateReservationAsManagerCommand>
{
    public UpdateReservationAsManagerCommandValidator()
    {
        Include(new UpdateReservationBaseCommandValidator());
    }
}