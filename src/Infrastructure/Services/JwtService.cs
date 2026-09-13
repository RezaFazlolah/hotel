using System.Security.Claims;
using System.Text;
using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common;

namespace Infrastructure.Services;

public class JwtService(
    IOptions<JwtSettings> jwtOptions,
    UserManager<User> userManager)
    : IJwtService
{
    public async Task<Result<string>> GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await userManager.GetRolesAsync(user);
        var rolesAsClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claims.AddRange(rolesAsClaims);

        var jwtSettings = jwtOptions.Value;
        var claimsAsDictionary = claims.ToDictionary(c => c.Type, object (c) => c.Value);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var now = DateTime.UtcNow;
        
        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            Claims = claimsAsDictionary,
            IssuedAt = now,
            Expires = now.AddMinutes(jwtSettings.DurationInMinutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(jwtDescriptor);
        return Result<string>.Success(token);
    }
}