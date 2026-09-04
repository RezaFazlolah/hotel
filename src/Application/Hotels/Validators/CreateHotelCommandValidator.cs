using Application.Hotels.Commands;
using Application.Hotels.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Application.Hotels.Validators;

public class CreateHotelCommandValidator
    : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator(IOptions<HotelSettings> hotelOptions)
    {
        var hotelSettings = hotelOptions.Value;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Rating)
            .ValidHotelRating(hotelSettings.MinRating, hotelSettings.MaxRating);
    }
}