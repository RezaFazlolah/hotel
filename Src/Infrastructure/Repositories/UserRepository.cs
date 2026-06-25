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

    public override Task<Result<User>> InsertAsync(User entity, CancellationToken ct)
        // throwing NotSupportedException is intentional, instead InsertAsync(User user, string password, CancellationToken ct) should be used for User
        => throw new NotSupportedException(
            "use UserRepository.InsertAsync(User user, string password, CancellationToken ct) instead.");

    public virtual async Task<Result> InsertAsync(User user, string password, CancellationToken ct)
    {
        var userCreateResult = (await userManager.CreateAsync(user, password));
        if (!userCreateResult.Succeeded)
        {
            var errors = userCreateResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            var errorsAsString = string.Join(". ", errors);
            return Result.Failure(new Error($"create user {user.PhoneNumber} failed. {errorsAsString}"));
        }
        
        return Result.Success();
    }

    public override Task<Result<User>> DeleteAsync(Guid id, CancellationToken ct)
        // throwing NotSupportedException is intentional, instead DeleteAsync(User user, CancellationToken ct) should be used for User 
        => throw new NotSupportedException("use UserRepository.DeleteAsync(User user, CancellationToken ct) instead.");

    public async Task<Result> DeleteAsync(User user, CancellationToken ct)
    {
        var userDeleteResult = await userManager.DeleteAsync(user);
        
        if (!userDeleteResult.Succeeded)
        {
            var errors = userDeleteResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            var errorsAsString = string.Join(". ", errors);
            return Result.Failure(new Error($"delete user {user.PhoneNumber} failed. {errorsAsString}."));
        }

        return Result.Success();
    }

    public virtual async Task<Result<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct)
    {
        var result = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);
        return result is null
            ? Result<User>.Failure(
                new Error($"{EntityName} with phone number {phoneNumber} not found", ErrorCode.NotFound),
                ResultCode.NotFound)
            : Result<User>.Success(result);
    }

    public virtual async Task<bool> CheckPassword(User user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    public virtual async Task<bool> RoleExistsAsync(UserRole role, CancellationToken ct)
        => await roleManager.RoleExistsAsync(role.ToString());

    public virtual async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(User user, CancellationToken ct)
        => Result<IEnumerable<UserRole>>.Success((await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>));

    public virtual async Task<Result<IEnumerable<UserRole>>> GetRolesAsync(Guid userId, CancellationToken ct)
    {
        var userResult = await GetByIdAsync(userId, ct);
        if (!userResult.Succeeded)
            return Result<IEnumerable<UserRole>>.Failure(userResult.Errors);
        var user = userResult.Value;
        return await GetRolesAsync(user, ct);
    }

    public virtual Task<Result<PagedResult<Reservation>>> GetAllReservationsAsync(Guid userId,
        PaginationParameters paginationParameters, CancellationToken ct)
        => throw new NotImplementedException();

    public async Task<Result> AddRoleAsync(User user, UserRole role, CancellationToken ct)
    {
        var roleAddResult = await userManager.AddToRoleAsync(user, role.ToString());

        if (!roleAddResult.Succeeded)
        {
            var errors = roleAddResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            var errorsAsString = string.Join(". ", errors);
            return Result.Failure(new Error($"add role {role.ToString()} to user {user.Id} failed. {errorsAsString}."));
        }

        return Result.Success();
    }

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