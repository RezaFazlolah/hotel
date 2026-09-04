using FluentValidation;
using SharedKernel.Enums;

namespace Application.Reservations.Validators;

public static class ReservationValidationExtensions
{
    extension<T>(IRuleBuilder<T, DateTimeOffset> ruleBuilder)
    {
        public IRuleBuilder<T, DateTimeOffset> ValidCheckInDate()
            => ruleBuilder
                .GreaterThanOrEqualTo(x => DateTimeOffset.UtcNow)
                .WithMessage("time travel hasn't been invented yet. CheckInDate cant be in the past");

        public IRuleBuilder<T, DateTimeOffset> ValidCheckOutDate(Func<T, DateTimeOffset> checkInDateSelector)
            => ruleBuilder
                .Must((command, checkOutDate) => checkOutDate > checkInDateSelector(command))
                .WithMessage("CheckOutDate must be after CheckInDate.");
    }

    extension<T>(IRuleBuilder<T, DateTimeOffset?> ruleBuilder)
    {
        public IRuleBuilder<T, DateTimeOffset?> ValidMinCheckInDate()
            => ruleBuilder
                .Must((_, _) => true);

        public IRuleBuilder<T, DateTimeOffset?> ValidMinCheckOutDate()
            => ruleBuilder
                .Must((_, _) => true);

        public IRuleBuilder<T, DateTimeOffset?> ValidMaxCheckInDate(Func<T, DateTimeOffset?> minCheckInDateSelector)
            => ruleBuilder
                .Must((command, maxCheckInDate) =>
                    maxCheckInDate is null
                    || minCheckInDateSelector(command) <= maxCheckInDate)
                .WithMessage("MinCheckInDate must be less than or equal to MaxCheckInDate.");

        public IRuleBuilder<T, DateTimeOffset?> ValidMaxCheckOutDate(Func<T, DateTimeOffset?> minCheckOutDateSelector)
            => ruleBuilder
                .Must((command, maxCheckOutDate) =>
                    maxCheckOutDate is null
                    || minCheckOutDateSelector(command) <= maxCheckOutDate)
                .WithMessage("MinCheckOutDate must be less than or equal to MaxCheckOutDate.");
    }

    extension<T>(IRuleBuilder<T, ReservationStatus> ruleBuilder)
    {
        public IRuleBuilder<T, ReservationStatus> ValidReservationStatus()
            => ruleBuilder
                .IsInEnum()
                .WithMessage($"ReservationStatus must be {string.Join(", ", Enum.GetNames<ReservationStatus>())}");
    }

    extension<T>(IRuleBuilder<T, ReservationStatus?> ruleBuilder)
    {
        public IRuleBuilder<T, ReservationStatus?> ValidReservationStatus()
            => ruleBuilder
                .Must((_, reservationStatus) =>
                    reservationStatus is null
                    || Enum.IsDefined<ReservationStatus>(reservationStatus.Value))
                .WithMessage($"ReservationStatus must be {string.Join(", ", Enum.GetNames<ReservationStatus>())}");
    }

    extension<T>(IRuleBuilder<T, decimal?> ruleBuilder)
    {
        public IRuleBuilder<T, decimal?> ValidTotalPrice()
            => ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage("TotalPrice must be greater than or equal to 0");
    }
}