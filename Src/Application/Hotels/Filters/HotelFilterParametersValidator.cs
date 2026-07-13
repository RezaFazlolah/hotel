using FluentValidation;

namespace Application.Hotels.Filters;

public class HotelFilterParametersValidator
    : AbstractValidator<HotelFilterParameters>
{
    public HotelFilterParametersValidator()
    {
        RuleFor(x => x.MinRating)
            .InclusiveBetween(0, 5)
            .When(x => x.MinRating.HasValue)
            .WithMessage("Min rating must be between 0 & 5");

        RuleFor(x => x.MaxRating)
            .InclusiveBetween(0, 5)
            .When(x => x.MaxRating.HasValue)
            .WithMessage("Min rating must be between 0 & 5");

        RuleFor(x=>x.MinRating)
            .LessThanOrEqualTo(x=>x.MaxRating)
            .When(x => x.MinRating.HasValue && x.MaxRating.HasValue)
            .WithMessage("MinRating must be less than or equal to MaxRating");
    }
}