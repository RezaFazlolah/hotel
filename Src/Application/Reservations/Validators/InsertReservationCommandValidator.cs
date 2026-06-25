using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class InsertReservationCommandValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationCommandValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate).WithMessage("CheckOutDate must be after CheckInDate");
    }
}