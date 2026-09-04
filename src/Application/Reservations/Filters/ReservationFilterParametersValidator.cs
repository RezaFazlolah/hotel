using Application.Reservations.Validators;
using FluentValidation;
using SharedKernel.Enums;

namespace Application.Reservations.Filters;

public class ReservationFilterParametersValidator
    : AbstractValidator<ReservationFilterParameters>
{
    public ReservationFilterParametersValidator()
    {
        RuleFor(x => x.MinCheckInDate)
            .ValidMinCheckInDate();
        RuleFor(x => x.MinCheckOutDate)
            .ValidMinCheckOutDate();
        RuleFor(x => x.MaxCheckInDate)
            .ValidMaxCheckInDate(x => x.MinCheckInDate);
        RuleFor(x => x.MaxCheckOutDate)
            .ValidMaxCheckOutDate(x => x.MinCheckOutDate);

        RuleFor(x => x.MinTotalPrice)
            .ValidTotalPrice();
        RuleFor(x => x.MaxTotalPrice)
            .ValidTotalPrice();
        RuleFor(x => x.MinTotalPrice)
            .LessThanOrEqualTo(x => x.MaxTotalPrice)
            .When(x => x.MinTotalPrice.HasValue && x.MaxTotalPrice.HasValue)
            .WithMessage("MinTotalPrice must be less than or equal to MaxTotalPrice");

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage($"Status must be {string.Join(", ", Enum.GetNames<ReservationStatus>())}");
    }
}