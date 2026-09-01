using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomCommandBaseValidator
    : AbstractValidator<UpdateRoomCommandBase>
{
    public UpdateRoomCommandBaseValidator()
    {
        RuleFor(x => x.Number)
            .ValidRoomNumber();

        RuleFor(x => x.Type)
            .ValidRoomType();

        RuleFor(x => x.PricePerNight)
            .ValidPricePerNight();
    }
}