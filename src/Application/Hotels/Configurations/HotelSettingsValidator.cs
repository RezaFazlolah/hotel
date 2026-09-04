using Microsoft.Extensions.Options;

namespace Application.Hotels.Configurations;

public class HotelSettingsValidator
    : IValidateOptions<HotelSettings>
{
    public ValidateOptionsResult Validate(
        string? name,
        HotelSettings options)
    {
        var errors = new List<string>();
        
        if(options.MinRating>options.MaxRating)
            errors.Add("MinRating must be less than or equal to MaxRating");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}