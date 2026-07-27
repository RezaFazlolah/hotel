using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common;
using SharedKernel.Configurations;

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
            new(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await userManager.GetRolesAsync(user);
        var rolesAsClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claims.AddRange(rolesAsClaims);

        var jwtSettings = jwtOptions.Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        var result = new JwtSecurityTokenHandler().WriteToken(token);
        return Result<string>.Success(result);
    }
}