using Application.Hotels.Validators;
using FluentValidation;

namespace Application.Hotels.Filters;

public class HotelFilterParametersValidator
    : AbstractValidator<HotelFilterParameters>
{
    public HotelFilterParametersValidator()
    {
        RuleFor(x => x.MinRating)
            .ValidHotelRating();

        RuleFor(x => x.MaxRating)
            .ValidHotelRating();

        RuleFor(x => x.MinRating)
            .LessThanOrEqualTo(x => x.MaxRating)
            .When(x => x.MinRating.HasValue && x.MaxRating.HasValue)
            .WithMessage("MinRating must be less than or equal to MaxRating");
    }
}