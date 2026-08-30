using FluentValidation;

namespace Application.Reservations.Validators;

public static class ReservationValidationExtensions
{
    extension<T>(IRuleBuilder<T, DateTimeOffset> ruleBuilder)
    {
        public IRuleBuilder<T, DateTimeOffset> ValidCheckInDate()
            => ruleBuilder.GreaterThanOrEqualTo(x => DateTimeOffset.UtcNow)
                .WithMessage("time travel hasn't been invented yet. CheckInDate cant be in the past");

        public IRuleBuilder<T, DateTimeOffset> ValidCheckOutDate(Func<T, DateTimeOffset> checkInDateSelector)
            => ruleBuilder.Must((command, checkOutDate) => checkOutDate > checkInDateSelector(command))
                .WithMessage("CheckOutDate must be after CheckInDate.");
    }
}