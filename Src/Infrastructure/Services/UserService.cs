using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace Infrastructure.Services;

public class UserService(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager
)
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