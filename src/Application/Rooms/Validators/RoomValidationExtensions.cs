using FluentValidation;
using SharedKernel.Enums;

namespace Application.Rooms.Validators;

public static class RoomValidationExtensions
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilder<T, int> ValidRoomNumber()
            => ruleBuilder.GreaterThan(0).WithMessage("RoomNumber must be positive");
    }

    extension<T>(IRuleBuilder<T, RoomType> ruleBuilder)
    {
        public IRuleBuilder<T, RoomType> ValidRoomType()
            => ruleBuilder.IsInEnum().WithMessage("RoomType must be an enum");
    }

    extension<T>(IRuleBuilder<T, decimal> ruleBuilder)
    {
        public IRuleBuilder<T, decimal> ValidPricePerNight()
            => ruleBuilder.GreaterThan(0).WithMessage("PricePerNight must be positive");
    }
}