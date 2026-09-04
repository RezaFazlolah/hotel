using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class CreateRoomCommandValidator
    : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("hotelId is required");

        RuleFor(x => x.Number)
            .ValidRoomNumber();

        RuleFor(x => x.Type)
            .ValidRoomType();

        RuleFor(x => x.PricePerNight)
            .ValidPricePerNight();
    }
}