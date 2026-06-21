using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class InsertReservationValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate).WithMessage("CheckOutDate must be after CheckInDate");
    }
}