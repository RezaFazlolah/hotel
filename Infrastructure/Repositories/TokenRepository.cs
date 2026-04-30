using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class TokenRepository(IConfiguration configuration) : ITokenRepository
{
    public string CreateJwt(User user, ICollection<string> roles)
    {
        var keyValue = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(150);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
