using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class CreateRoomCommandValidator
    : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(c => c.HotelId)
            .NotEmpty()
            .WithMessage("hotelId is required");

        RuleFor(c => c.Number)
            .ValidRoomNumber();

        RuleFor(c => c.Type)
            .ValidRoomType();

        RuleFor(c => c.PricePerNight)
            .ValidPricePerNight();
    }
}