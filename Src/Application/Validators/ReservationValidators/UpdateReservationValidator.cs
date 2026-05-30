using Application.Requests.ReservationRequests;
using FluentValidation;

namespace Application.Validators.ReservationValidators;

public class UpdateReservationValidator : AbstractValidator<UpdateReservation>
{
    public UpdateReservationValidator()
    {
        RuleFor(c => c.CheckOutDate)
            .LessThanOrEqualTo(c => c.CheckInDate).WithMessage("CheckOutDate must be after CheckInDate");
    }
}