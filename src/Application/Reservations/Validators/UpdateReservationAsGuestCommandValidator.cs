using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationAsGuestCommandValidator
    :AbstractValidator<UpdateReservationAsGuestCommand>
{
    public UpdateReservationAsGuestCommandValidator()
    {
        RuleFor(x => x.CheckInDate)
            .ValidCheckInDate();

        RuleFor(x => x.CheckOutDate)
            .ValidCheckOutDate(x => x.CheckInDate);
    }
}