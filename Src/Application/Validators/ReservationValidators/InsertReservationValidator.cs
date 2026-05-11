using Application.Commands.ReservationCommands;
using FluentValidation;

namespace Application.Validators.ReservationValidators;

public class InsertReservationValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate).WithMessage("CheckOutDate must be after CheckInDate");
    }
}