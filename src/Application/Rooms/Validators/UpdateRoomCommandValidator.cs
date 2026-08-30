using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class UpdateRoomCommandValidator
    : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
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