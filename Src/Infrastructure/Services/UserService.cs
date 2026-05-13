using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace Infrastructure.Services;

public abstract class UserService(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager
)
    : BaseService<Guid, User>(context), IUserService
{
    public virtual async Task<bool> ExistsAsync(string phoneNumber, CancellationToken ct)
        => await GetByPhoneNumberAsync(phoneNumber, ct) != null;

    public override Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public override async Task<User?> InsertAsync(User entity, CancellationToken ct)
    // throwing NotSupportedException is intentional, InsertAsync(User, string, CancellationToken) should be used for User
        => throw new NotSupportedException();

    public virtual async Task<User?> InsertAsync(User user, string password, CancellationToken ct)
        => (await userManager.CreateAsync(user, password)).Succeeded
                ? await GetByPhoneNumberAsync(user.PhoneNumber, ct)
                : null;

    public virtual async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct)
        => await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);

    public virtual async Task<bool> PasswordChecks(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public virtual async Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct)
        => await roleManager.RoleExistsAsync(role.ToString());
    
    public virtual async Task<IEnumerable<UserRole>> GetRolesAsync(User user, CancellationToken ct)
        => (await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>);

    public virtual async Task<IEnumerable<UserRole>> GetRolesAsync(Guid userId, CancellationToken ct)
    {
        var user = await GetByIdAsync(userId, ct);
        if (user == null)
            return [];
        return await GetRolesAsync(user, ct);
    }

    public abstract Task<ICollection<Reservation>> GetReservationsAsync(Guid userId,
        CancellationToken ct);

    protected override IQueryable<User> CustomContext()
        => throw new NotImplementedException();
    
    protected override IQueryable<User> CustomFilter(IQueryable<User> query, string? filterOn, string? filterQuery)
        => throw new NotImplementedException();
    
    protected override IQueryable<User> CustomSort(IQueryable<User> query, string? orderBy, bool isAscending)
        => throw new NotImplementedException();

    public override async Task<User> GetByIdAsync(Guid id, CancellationToken ct)
        => await userManager.Users.SingleAsync(u => id == u.Id, ct);
}