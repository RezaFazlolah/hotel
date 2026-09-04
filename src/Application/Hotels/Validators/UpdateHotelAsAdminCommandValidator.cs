using Application.Hotels.Commands;
using Application.Hotels.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Application.Hotels.Validators;

public class UpdateHotelAsAdminCommandValidator
    : AbstractValidator<UpdateHotelAsAdminCommand>
{
    public UpdateHotelAsAdminCommandValidator(IOptions<HotelSettings> hotelOptions)
    {
        var hotelSettings = hotelOptions.Value;

        Include(new UpdateHotelBaseCommandValidator());

        RuleFor(x => x.Rating)
            .ValidHotelRating(hotelSettings.MinRating, hotelSettings.MaxRating);
    }
}