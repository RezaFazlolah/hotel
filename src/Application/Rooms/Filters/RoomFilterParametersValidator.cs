using Application.Rooms.Validators;
using FluentValidation;
using SharedKernel.Enums;

namespace Application.Rooms.Filters;

public class RoomFilterParametersValidator
    : AbstractValidator<RoomFilterParameters>
{
    public RoomFilterParametersValidator()
    {
        // Number
        RuleFor(x => x.MinNumber)
            .ValidRoomNumber();
        RuleFor(x => x.MaxNumber)
            .ValidRoomNumber();
        RuleFor(x=>x.MinNumber)            
            .LessThanOrEqualTo(x=> x.MaxNumber)
            .When(x => x.MinNumber.HasValue &&  x.MaxNumber.HasValue)
            .WithMessage("MinNumber must be less than or equal to MaxNumber");
        
        // Type
        RuleFor(x => x.Type)
            .ValidRoomType();
        
        // PricePerNight
        RuleFor(x => x.MinPricePerNight)
            .ValidPricePerNight();

        RuleFor(x => x.MaxPricePerNight)
            .ValidPricePerNight();

        RuleFor(x => x.MinPricePerNight)
            .LessThanOrEqualTo(x => x.MaxPricePerNight)
            .When(x => x.MinPricePerNight.HasValue && x.MaxPricePerNight.HasValue)
            .WithMessage("MinPricePerNight be less than or equal to MaxPricePerNight");
    }
}