using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate).WithMessage("CheckOutDate must be after CheckInDate");
    }
}