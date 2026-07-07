using FluentValidation;

namespace Application.Reservations.Commands.Validators;

public class InsertReservationCommandValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationCommandValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .GreaterThanOrEqualTo(c => c.CheckInDate)
            .WithMessage("CheckOutDate must be after CheckInDate.");
    }
}