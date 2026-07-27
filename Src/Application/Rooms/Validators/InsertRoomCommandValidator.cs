using Application.Rooms.Commands;
using FluentValidation;

namespace Application.Rooms.Validators;

public class InsertRoomCommandValidator
    : AbstractValidator<InsertRoomCommand>
{
    public InsertRoomCommandValidator()
    {
        RuleFor(c => c.Number)
            .GreaterThan(0).
            WithMessage("room Number must be positive");
        
        RuleFor(c => c.Type)
            .IsInEnum().
            WithMessage("RoomType must be enum, 0 for Normal and 1 for Vip");
        
        RuleFor(c => c.PricePerNight)
            .GreaterThan(0)
            .WithMessage("room PriceperPricePerNight must be positive");
        
        RuleFor(c => c.HotelId)
            .NotEmpty()
            .WithMessage("hotelId is required");
    }
}