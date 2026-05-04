using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class UserService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration)
    : IUserService
{
    public async Task<bool> UserExistsAsync(string phoneNumber, CancellationToken cancellationToken)
        => await GetByPhoneNumberAsync(phoneNumber, cancellationToken) != null;

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        => await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

    public async Task<bool> PasswordChecks(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public async Task<IdentityResult> RegisterAsync(User user, string password, string role,
        CancellationToken cancellationToken)
    {
        if (!await roleManager.RoleExistsAsync(role))
            return IdentityResult.Failed(new IdentityError {Description = $"role {role} not found"});            
            // return await userManager.AddToRoleAsync(user, role); // this is only for returning error

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return result;
        return await userManager.AddToRoleAsync(user, role);
    }

    public async Task<string?> GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var expirationDuration = configuration["Jwt:DurationInMinutes"] ??
                                 throw new InvalidOperationException("Jwt:DurationInMinutes is missing");

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(expirationDuration)),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        => await userManager.GetRolesAsync(user);
}