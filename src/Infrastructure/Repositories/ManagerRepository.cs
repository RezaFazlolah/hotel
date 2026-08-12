using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager)
    : UserRepository(db, userManager),
        IManagerRepository
{
    public override async Task<bool> ExistsAsync(Guid managerId, CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId, ct);

    public async Task<Result<Guid?>> GetHotelIdAsync(
        Guid managerId,
        CancellationToken ct)
    {
        var result = await db.Managers
            .Where(m => m.Id == managerId)
            .Select(m => new
            {
                HotelId = m.HotelId
            })
            .FirstOrDefaultAsync(ct);

        if (result is null)
            return Result<Guid?>.Failure(new Error($"manager {managerId} not found", ErrorCode.NotFound),
                ResultCode.NotFound);
        return Result<Guid?>.Success(result.HotelId);
    }

    public async Task<bool> ManagesHotel(
        Guid managerId,
        Guid hotelId,
        CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId
                           && m.HotelId == hotelId, cancellationToken: ct);

    // RoomRepository.IsManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct) does the same thing
    public async Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId
                           && m.Hotel != null
                           && m.Hotel.Rooms.Any(r => r.Id == roomId), ct);
}