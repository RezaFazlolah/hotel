using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationCommandValidator
    : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(c => c.CheckInDate)
            .ValidCheckInDate();

        RuleFor(c => c.CheckOutDate)
            .ValidCheckOutDate(x => x.CheckInDate);
    }
}