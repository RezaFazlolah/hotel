using Application.Reservations.Commands;
using FluentValidation;

namespace Application.Reservations.Validators;

public class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(x => DateTimeOffset.Now)
            .WithMessage("time travel hasn't been invented yet. CheckInDate cant be in the past");
        
        RuleFor(c => c.CheckOutDate)
            .GreaterThan(c => c.CheckInDate)
            .WithMessage("CheckOutDate must be after CheckInDate.");
    }
}