using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IManagerRepository
{
    public override async Task<bool> ExistsAsync(Guid managerId, CancellationToken ct)
        => await db.Managers.AnyAsync(g => g.Id == managerId, ct);

    // Performance: fetch only HotelId column instead of fetching all columns and returning only HotelId 
    public async Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var managerResult = await GetByIdAsync(managerId, ct);
        if (!managerResult.Succeeded)
            return Result<Guid?>.Failure(managerResult.Errors);
        var manager = (Manager)managerResult.Value;

        return Result<Guid?>.Success(manager.HotelId);

        // var hotelId = await db.Managers
        //     .Where(m => m.Id == managerId)
        //     .Select(m => m.HotelId)
        //     .FirstOrDefaultAsync(ct);
    }
}