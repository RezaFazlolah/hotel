using System.Text;
using Microsoft.Extensions.Options;

namespace Infrastructure.Jwt;

public class JwtSettingsValidator
    :IValidateOptions<JwtSettings>
{
    public ValidateOptionsResult Validate(
        string? name,
        JwtSettings options)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(options.Key))
            errors.Add("Key is missing");
        else if (Encoding.UTF8.GetByteCount(options.Key) < 32)
            errors.Add("Key must be at least 32 bytes for HMAC-SHA256");
        
        if (string.IsNullOrWhiteSpace(options.Issuer))
            errors.Add("Issuer is missing");
        
        if (string.IsNullOrWhiteSpace(options.Audience))
            errors.Add("Audience is missing");
        
        if(options.DurationInMinutes<=0)
            errors.Add("DurationInMinutes must be greater than 0");
        
        if(options.ClockSkewInMinutes < 0)
            errors.Add("ClockSkew must be  greater than or equal to 0");
        
        return errors.Count>0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}