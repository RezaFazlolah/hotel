using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.CheckInDate)
            .ValidCheckInDate();

        RuleFor(x => x.CheckOutDate)
            .ValidCheckOutDate(x=>x.CheckInDate);
    }
}