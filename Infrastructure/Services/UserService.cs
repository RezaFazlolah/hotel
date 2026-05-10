using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class UserService(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IConfiguration configuration)
    : BaseService<Guid, User>(context), IUserService
{
    public async Task<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken)
        => await GetByPhoneNumberAsync(phoneNumber, cancellationToken) != null;

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        => await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

    public async Task<bool> PasswordChecks(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public async Task<IdentityResult> RegisterAsync(User user, string password, UserRole role,
        CancellationToken cancellationToken)
    {
        if (!await roleManager.RoleExistsAsync(role.ToString()))
            return IdentityResult.Failed(new IdentityError { Description = $"role {role} not found" });

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return result;
        return await userManager.AddToRoleAsync(user, role.ToString());
    }

    public async Task<string?> GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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

    public async Task<IEnumerable<UserRole>> GetRolesAsync(User user, CancellationToken cancellationToken)
        => (await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>);

    public async Task<IEnumerable<UserRole>> GetRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return [];
        return await GetRolesAsync(user, cancellationToken);
    }

    protected override IQueryable<User> CustomContext()
        => throw new NotImplementedException();

    protected override IQueryable<User> CustomFilter(IQueryable<User> query, string? filterOn, string? filterQuery)
        => throw new NotImplementedException();

    protected override IQueryable<User> CustomSort(IQueryable<User> query, string? orderBy, bool isAscending)
        => throw new NotImplementedException();

    public override async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await userManager.Users.FirstOrDefaultAsync(u => id == u.Id, cancellationToken);
}