using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class UserRepository(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : BaseRepository<Guid, User>(context), IUserRepository
{
    public virtual async Task<bool> ExistsAsync(string phoneNumber, CancellationToken ct)
        => await userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber, ct);

    public override async Task<Result<User>> InsertAsync(User entity, CancellationToken ct)
        // throwing NotSupportedException is intentional, InsertAsync(User, string, CancellationToken) should be used for User
        => throw new NotSupportedException();

    public virtual async Task<Result<User>> InsertAsync(User user, string password, CancellationToken ct)
        => (await userManager.CreateAsync(user, password)).Succeeded
            ? Result<User>.Success((await GetByPhoneNumberAsync(user.PhoneNumber, ct)).Value)
            : Result<User>.Failure(new Error($"insert failed"));

    public virtual async Task<Result<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct)
    {
        var result = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);
        return result is null
            ? Result<User>.Failure(
                new Error($"{EntityName} with phone number {phoneNumber} not found", ErrorCode.NotFound),
                ResultCode.NotFound)
            : Result<User>.Success(result);
    }

    public virtual async Task<bool> PasswordChecks(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public virtual async Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct)
        => await roleManager.RoleExistsAsync(role.ToString());

    public virtual async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(User user, CancellationToken ct)
        => Result<IEnumerable<UserRole>>.Success((await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>));

    public virtual async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(Guid userId, CancellationToken ct)
    {
        var result = await GetByIdAsync(userId, ct);
        if (!result.Succeeded)
            return Result<IEnumerable<UserRole>>.Failure(result.Errors);
        return await GetRolesAsync(result.Value, ct);
    }

    public virtual Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid userId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => throw new NotImplementedException();

    protected override IQueryable<User> CustomContext()
        => userManager.Users;

    public override async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await userManager.Users.SingleOrDefaultAsync(u => id == u.Id, ct);
            return result is null
                ? Result<User>.Failure(new Error($"{EntityName} with id {id} not found", ErrorCode.NotFound),
                    ResultCode.NotFound)
                : Result<User>.Success(result);
        }
        catch
        {
            return Result<User>.Failure(new Error($"more than 1 {EntityName} with id {id} found"));
        }
    }
}