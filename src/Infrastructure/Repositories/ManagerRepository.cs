using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class ManagerRepository(
    AppDbContext db,
    UserManager<User> userManager,
    IDistributedCache cache)
    : UserRepository(db, userManager, cache),
        IManagerRepository
{
    public override async Task<bool> ExistsAsync(Guid managerId, CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId, ct);

    public async Task<Result<Manager?>> GetByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
    {
        var result = await db.Managers.FirstOrDefaultAsync(m => m.HotelId == hotelId, ct);
        return Result<Manager?>.Success(result);
    }

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

    public async Task<bool> ManagesHotelAsync(
        Guid managerId,
        Guid hotelId,
        CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId
                           && m.HotelId == hotelId, cancellationToken: ct);

    // same as RoomRepository.IsManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct)
    public async Task<bool> ManagesRoomAsync(
        Guid managerId,
        Guid roomId,
        CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId
                           && m.Hotel != null
                           && m.Hotel.Rooms.Any(r => r.Id == roomId), ct);

    // same as ReservationRepository.IsManagedByManager(Guid reservationId, Guid managerId, CancellationToken ct)
    public async Task<bool> ManagesReservationAsync(
        Guid managerId,
        Guid reservationId,
        CancellationToken ct)
        => await db.Managers
            .AnyAsync(m => m.Id == managerId
                           && m.Hotel != null
                           && m.Hotel.Rooms
                               .Any(rm => rm.Reservations
                                   .Any(rz => rz.Id == reservationId)), ct);
}