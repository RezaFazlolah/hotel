using FluentValidation;

namespace Application.Reservations.Filters;

public class ReservationFilterParametersValidator
    : AbstractValidator<ReservationFilterParameters>
{
    public ReservationFilterParametersValidator()
    {
        RuleFor(x => x.MinCheckInDate)
            .LessThanOrEqualTo(x => x.MaxCheckInDate)
            .When(x => x.MinCheckInDate.HasValue && x.MaxCheckInDate.HasValue)
            .WithMessage("MinCheckInDate must be less than or equal to MaxCheckInDate.");

        RuleFor(x => x.MinCheckOutDate)
            .LessThanOrEqualTo(x => x.MaxCheckOutDate)
            .When(x => x.MinCheckOutDate.HasValue && x.MaxCheckOutDate.HasValue)
            .WithMessage("MinCheckOutDate must be less than or equal to MaxCheckOutDate.");

        RuleFor(x => x.MinTotalPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinTotalPrice.HasValue)
            .WithMessage("MinTotalPrice cannot be negative.");

        RuleFor(x => x.MaxTotalPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxTotalPrice.HasValue)
            .WithMessage("MaxTotalPrice cannot be negative.");

        RuleFor(x => x.MinTotalPrice)
            .LessThanOrEqualTo(x => x.MaxTotalPrice)
            .When(x => x.MinTotalPrice.HasValue && x.MaxTotalPrice.HasValue)
            .WithMessage("MinTotalPrice must be less than or equal to MaxTotalPrice.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);
    }
}