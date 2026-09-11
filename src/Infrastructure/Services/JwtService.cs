using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Common;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

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
        var claimsAsDictionary = claims.ToDictionary(c => c.Type, object (c) => c.Value);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var jwtDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            Claims = claimsAsDictionary,
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            NotBefore = DateTime.UtcNow
        };

        var jwtHandler = new JsonWebTokenHandler();
        var jwt = jwtHandler.CreateToken(jwtDescriptor);
        return Result<string>.Success(jwt);
    }
}