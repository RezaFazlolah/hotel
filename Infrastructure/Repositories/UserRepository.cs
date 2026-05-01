using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager, IConfiguration configuration)
    : IUserRepository
{
    public Dictionary<UserRoles, string> RolesToString { get; } = new()
    {
        [UserRoles.Admin] = "Admin",
        [UserRoles.Guest] = "Guest",
    };

    private Dictionary<string, UserRoles> StringToRoles =>
        RolesToString.ToDictionary(pair => pair.Value, pair => pair.Key);

    public async Task<bool> UserExistsAsync(string phoneNumber, CancellationToken cancellationToken)
        => await GetByPhoneNumberAsync(phoneNumber, cancellationToken) != null;

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        => await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

    public async Task<bool> PasswordChecks(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public async Task<bool> RegisterAsync(User user, string password, UserRoles userRole,
        CancellationToken cancellationToken)
    {
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return false;
        result = await userManager.AddToRoleAsync(user, RolesToString[userRole]);
        return result.Succeeded;
    }

    public async Task<string?> CreateJwt(User user)
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

        var roles = await userManager.GetRolesAsync(user);
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

    public async Task<IEnumerable<UserRoles>> GetRolesAsync(User user, CancellationToken cancellationToken)
        => (await userManager.GetRolesAsync(user)).Select(r => StringToRoles[r]).ToList();
}