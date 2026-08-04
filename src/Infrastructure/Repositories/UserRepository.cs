using Application.Interfaces.Repositories;
using Application.Users.Filters;
using Application.Users.Sorts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class UserRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : BaseRepository<Guid, User, UserFilterParameters, UserSortParameters>(db),
        IUserRepository
{
    public virtual async Task<bool> ExistsAsync(
        string phoneNumber,
        CancellationToken ct)
        => await userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber, ct);

    public override Task<Result<User>> InsertAsync(
            User entity,
            CancellationToken ct)
        // throwing NotSupportedException is intentional, instead InsertAsync(User user, string password, CancellationToken ct) should be used for User
        => throw new NotSupportedException(
            "use UserRepository.InsertAsync(User user, string password, CancellationToken ct) instead");

    public virtual async Task<Result> InsertAsync(
        User user,
        string password,
        CancellationToken ct)
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

    public override Task<Result<User>> DeleteAsync(
            Guid id,
            CancellationToken ct)
        // throwing NotSupportedException is intentional, instead DeleteAsync(User user, CancellationToken ct) should be used for User 
        => throw new NotSupportedException("use UserRepository.DeleteAsync(User user, CancellationToken ct) instead");

    public async Task<Result> DeleteAsync(
        User user,
        CancellationToken ct)
    {
        var userDeleteResult = await userManager.DeleteAsync(user);

        if (!userDeleteResult.Succeeded)
        {
            var errors = userDeleteResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            var errorsAsString = string.Join(". ", errors);
            return Result.Failure(new Error($"delete user {user.PhoneNumber} failed. {errorsAsString}"));
        }

        return Result.Success();
    }

    public virtual async Task<Result<User>> GetByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken ct)
    {
        var result = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);
        return result is null
            ? Result<User>.Failure(
                new Error($"{EntityName} with phone number {phoneNumber} not found", ErrorCode.NotFound),
                ResultCode.NotFound)
            : Result<User>.Success(result);
    }

    public virtual async Task<bool> CheckPassword(
        User user,
        string password)
        => await userManager.CheckPasswordAsync(user, password);

    public virtual async Task<Result<IReadOnlyList<UserRole>>> GetRolesAsync(
        User user,
        CancellationToken ct)
        => Result<IReadOnlyList<UserRole>>.Success(
            (await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>).ToList()
        );

    public virtual async Task<Result<IReadOnlyList<UserRole>>> GetRolesAsync(
        Guid userId,
        CancellationToken ct)
    {
        var userResult = await GetByIdAsync(userId, ct);
        if (!userResult.Succeeded)
            return Result<IReadOnlyList<UserRole>>.Failure(userResult.Errors);
        var user = userResult.Value;

        return await GetRolesAsync(user, ct);
    }

    public async Task<Result> AddRoleAsync(
        User user,
        UserRole role,
        CancellationToken ct)
    {
        var roleAddResult = await userManager.AddToRoleAsync(user, role.ToString());

        if (roleAddResult.Succeeded)
            return Result.Success();

        var errors = roleAddResult.Errors.Select(e => $"{e.Code}: {e.Description}");
        var errorsAsString = string.Join(". ", errors);

        return Result.Failure(
            new Error($"add role {role.ToString()} to user {user.Id} failed. {errorsAsString}")
        );
    }

    protected override IQueryable<User> CustomContext()
        => userManager.Users;

    public override async Task<Result<User>> GetByIdAsync(
        Guid id,
        CancellationToken ct)
    {
            var result = await userManager.Users.SingleOrDefaultAsync(u => id == u.Id, ct);
            return result is null
                ? Result<User>.Failure(new Error($"{EntityName} with id {id} not found", ErrorCode.NotFound),
                    ResultCode.NotFound)
                : Result<User>.Success(result);
    }
}