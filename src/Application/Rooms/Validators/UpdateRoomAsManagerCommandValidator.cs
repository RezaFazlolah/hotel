using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomAsManagerCommandValidator
    : AbstractValidator<UpdateRoomAsManagerCommand>
{
    public UpdateRoomAsManagerCommandValidator()
    {
        RuleFor(x => x.Number)
            .ValidRoomNumber();

        RuleFor(x => x.Type)
            .ValidRoomType();

        RuleFor(x => x.PricePerNight)
            .ValidPricePerNight();
    }
}