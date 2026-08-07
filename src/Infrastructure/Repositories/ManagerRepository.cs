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

    public async Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var manager = await db.Managers.FirstOrDefaultAsync(m => m.Id == managerId, ct);
        return manager is null
            ? Result<Guid?>.Failure(new Error($"manager {managerId} not found"))
            : Result<Guid?>.Success(manager.HotelId);
    }

    public async Task<Result<Guid>> GetIdByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
    {
        var result = await db.Managers.FirstOrDefaultAsync(m => m.HotelId == hotelId, ct);
        return result is null
            ? Result<Guid>.Failure(new Error($"hotel {hotelId} not found or isn't managed by any manager"))
            : Result<Guid>.Success(result.Id);
    }
}