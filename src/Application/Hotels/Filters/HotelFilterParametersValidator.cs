using Application.Hotels.Configurations;
using Application.Hotels.Validators;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Application.Hotels.Filters;

public class HotelFilterParametersValidator
    : AbstractValidator<HotelFilterParameters>
{
    public HotelFilterParametersValidator(IOptions<HotelSettings> hotelOptions)
    {
        var hotelSettings = hotelOptions.Value;

        RuleFor(x => x.MinRating)
            .ValidHotelRating(hotelSettings.MinRating, hotelSettings.MaxRating);

        RuleFor(x => x.MaxRating)
            .ValidHotelRating(hotelSettings.MinRating, hotelSettings.MaxRating);

        RuleFor(x => x.MinRating)
            .LessThanOrEqualTo(x => x.MaxRating)
            .When(x => x.MinRating.HasValue && x.MaxRating.HasValue)
            .WithMessage("MinRating must be less than or equal to MaxRating");
    }
}