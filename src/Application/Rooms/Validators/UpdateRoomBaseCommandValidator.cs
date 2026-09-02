using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomBaseCommandValidator
    : AbstractValidator<UpdateRoomBaseCommand>
{
    public UpdateRoomBaseCommandValidator()
    {
        RuleFor(x => x.Number)
            .ValidRoomNumber();

        RuleFor(x => x.Type)
            .ValidRoomType();

        RuleFor(x => x.PricePerNight)
            .ValidPricePerNight();
    }
}