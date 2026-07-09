using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IManagerRepository
{
    public async Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var managerResult = await GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            return Result<Guid?>.Failure(managerResult.Errors.Prepend(new Error(
                $"get manager {managerId}'s hotel ID failed.")));
        var manager = (Manager)managerResult.Value;

        return Result<Guid?>.Success(manager.HotelId);
    }
}