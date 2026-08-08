using System.Text;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Jwt;

public class JwtSettingsValidator
    : IValidateOptions<JwtSettings>
{
    public ValidateOptionsResult Validate(
        string? name,
        JwtSettings options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.Key))
            failures.Add("JwtSettings:Key is required.");
        else if (Encoding.UTF8.GetByteCount(options.Key) < 32)
            failures.Add("JwtSettings:Key must be at least 32 bytes for HMAC-SHA256.");

        if (string.IsNullOrWhiteSpace(options.Issuer))
            failures.Add("JwtSettings:Issuer is required.");

        if (string.IsNullOrWhiteSpace(options.Audience))
            failures.Add("JwtSettings:Audience is required.");

        if (options.DurationInMinutes <= 0)
            failures.Add("JwtSettings:DurationInMinutes must be greater than 0.");

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}