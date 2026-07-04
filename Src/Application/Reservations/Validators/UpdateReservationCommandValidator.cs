using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate)
            .WithMessage("CheckOutDate must be after CheckInDate");
    }
}