using FluentValidation;

namespace Application.Hotels.Validators;

public static class HotelValidationExtensions
{
    extension<T>(IRuleBuilder<T, decimal> ruleBuilder)
    {
        public IRuleBuilder<T, decimal> ValidHotelRating()
            => ruleBuilder
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");
    }
    extension<T>(IRuleBuilder<T, decimal?> ruleBuilder)
    {
        public IRuleBuilder<T, decimal?> ValidHotelRating()
            => ruleBuilder
                .Must((_, rating) => rating is null or >= 1 and <= 5)
                .WithMessage("Rating must be between 1 and 5");
    }
}