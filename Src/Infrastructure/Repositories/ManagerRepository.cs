using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
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