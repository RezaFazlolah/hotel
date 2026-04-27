using Application.Commands.ReservationCommands;
using FluentValidation;

namespace Application.Validators.ReservationValidators;

public class UpdateReservationValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationValidator()
    {
        RuleFor(c => c.CheckInDate)
            .NotEmpty().WithMessage("CheckInDate is required")
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("update CheckInDate cant be in the past");
        RuleFor(c => c.CheckOutDate)
            .NotEmpty().WithMessage("CheckOutDate is required")
            .GreaterThanOrEqualTo(c => c.CheckInDate).WithMessage("update CheckOutDate must be after CheckInDate");
        RuleFor(c => c.GuestId)
            .NotEmpty().WithMessage("update GuestId is required");
        RuleFor(c => c.RoomId)
            .NotEmpty().WithMessage("update RoomId is required");
    }
}