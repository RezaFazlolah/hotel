using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationAsGuestCommandValidator
    :AbstractValidator<UpdateReservationAsGuestCommand>
{
    public UpdateReservationAsGuestCommandValidator()
    {
        Include(new UpdateReservationBaseCommandValidator());
    }
}