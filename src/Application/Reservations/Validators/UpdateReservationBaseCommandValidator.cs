using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationBaseCommandValidator
    : AbstractValidator<UpdateReservationBaseCommand>
{
    public UpdateReservationBaseCommandValidator()
    {
        RuleFor(x => x.CheckInDate)
            .ValidCheckInDate();

        RuleFor(x => x.CheckOutDate)
            .ValidCheckOutDate(x => x.CheckInDate);
    }
}