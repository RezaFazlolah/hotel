using FluentValidation;

namespace Application.Hotels.Validators;

public static class HotelValidationExtensions
{
    extension<T>(IRuleBuilder<T, decimal> ruleBuilder)
    {
        public IRuleBuilder<T, decimal> ValidHotelRating(int minRating, int maxRating)
            => ruleBuilder
                .InclusiveBetween(minRating, maxRating)
                .WithMessage("Rating must be between 1 and 5");
    }

    extension<T>(IRuleBuilder<T, decimal?> ruleBuilder)
    {
        public IRuleBuilder<T, decimal?> ValidHotelRating(int minRating, int maxRating)
            => ruleBuilder
                .Must((_, rating) =>
                    rating is null
                    || (minRating <= rating && rating <= maxRating))
                .WithMessage($"Rating must be between {minRating} and {maxRating}");
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilder<T, string> ValidHotelName()
            => ruleBuilder.
                MaximumLength(200)
                .WithMessage("max Name length is 200");
        
        public IRuleBuilder<T, string> ValidHotelAddress()
            => ruleBuilder.
                MaximumLength(500)
                .WithMessage("max Address length is 500");
    }
}