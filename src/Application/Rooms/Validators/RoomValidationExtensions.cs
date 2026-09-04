using FluentValidation;
using SharedKernel.Enums;

namespace Application.Rooms.Validators;

public static class RoomValidationExtensions
{
    extension<T>(IRuleBuilder<T, int> ruleBuilder)
    {
        public IRuleBuilder<T, int> ValidRoomNumber()
            => ruleBuilder
                .GreaterThan(0)
                .WithMessage("RoomNumber must be greater than or equal to 0");
    }

    extension<T>(IRuleBuilder<T, int?> ruleBuilder)
    {
        public IRuleBuilder<T, int?> ValidRoomNumber()
            => ruleBuilder
                .Must((_, roomNumber) => roomNumber is null or > 0)
                .WithMessage("RoomNumber must be greater than or equal to 0");
    }

    extension<T>(IRuleBuilder<T, RoomType> ruleBuilder)
    {
        public IRuleBuilder<T, RoomType> ValidRoomType()
            => ruleBuilder
                .IsInEnum()
                .WithMessage($"RoomType must be {string.Join(", ", Enum.GetNames<RoomType>())}");
    }

    extension<T>(IRuleBuilder<T, RoomType?> ruleBuilder)
    {
        public IRuleBuilder<T, RoomType?> ValidRoomType()
            => ruleBuilder
                .Must((_, roomType) => roomType is null || Enum.IsDefined<RoomType>(roomType.Value))
                .WithMessage($"RoomType must be {string.Join(", ", Enum.GetNames<RoomType>())}");
    }

    extension<T>(IRuleBuilder<T, decimal> ruleBuilder)
    {
        public IRuleBuilder<T, decimal> ValidPricePerNight()
            => ruleBuilder
                .GreaterThan(0)
                .WithMessage("PricePerNight must be greater than or equal to 0");
    }

    extension<T>(IRuleBuilder<T, decimal?> ruleBuilder)
    {
        public IRuleBuilder<T, decimal?> ValidPricePerNight()
            => ruleBuilder
                .Must((_, roomPricePerNight) => roomPricePerNight is null or > 0)
                .WithMessage("PricePerNight must be greater than or equal to 0");
    }
}