using Application.Commands.ReservationCommands;
using FluentValidation;

namespace Application.Validators.ReservationValidators;

public class InsertReservationValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationValidator()
    {
        RuleFor(c => c.CheckInDate)
            .NotEmpty().WithMessage("CheckInDate is required")
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("insert CheckInDate cant be in the past");
        RuleFor(c => c.CheckOutDate)
            .NotEmpty().WithMessage("CheckOutDate is required")
            .GreaterThanOrEqualTo(c => c.CheckInDate).WithMessage("insert CheckOutDate must be after CheckInDate");
        RuleFor(c => c.GuestId)
            .NotEmpty().WithMessage("insert GuestId is required");
        RuleFor(c => c.RoomId)
            .NotEmpty().WithMessage("insert RoomId is required");
    }
}