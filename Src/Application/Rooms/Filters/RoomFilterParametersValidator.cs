using FluentValidation;

namespace Application.Rooms.Filters;

public class RoomFilterParametersValidator
    : AbstractValidator<RoomFilterParameters>
{
    public RoomFilterParametersValidator()
    {
        // Number
        RuleFor(x=>x.MinNumber)
            .GreaterThanOrEqualTo(1)
            .When(x => x.MinNumber.HasValue)
            .WithMessage("MinNumber must be greater than or equal to 1");

        RuleFor(x => x.MaxNumber)
            .GreaterThanOrEqualTo(1)
            .When(x => x.MaxNumber.HasValue)
            .WithMessage("MaxNumber must be greater than or equal to 1");

        RuleFor(x=>x.MinNumber)            
            .LessThanOrEqualTo(x=> x.MaxNumber)
            .When(x => x.MinNumber.HasValue &&  x.MaxNumber.HasValue)
            .WithMessage("MaxNumber must be greater than or equal to MinNumber");
        
        // Type
        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage("enter valid RoomType");
        
        // PricePerNight
        RuleFor(x => x.MinPricePerNight)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPricePerNight.HasValue)
            .WithMessage("MinPricePerNight must be greater than or equal to 0");

        RuleFor(x => x.MaxPricePerNight)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPricePerNight.HasValue)
            .WithMessage("MaxPricePerNight must be greater than or equal to 1");

        RuleFor(x => x.MinPricePerNight)
            .LessThanOrEqualTo(x => x.MaxPricePerNight)
            .When(x => x.MinPricePerNight.HasValue && x.MaxPricePerNight.HasValue)
            .WithMessage("MaxPricePerNight be greater than or equal to MinPricePerNight");
    }
}