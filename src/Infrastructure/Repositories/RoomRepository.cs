using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class RoomRepository(
    AppDbContext db,
    IHotelRepository hotelRepository,
    IDistributedCache cache)
    : RepositoryBase<Guid, Room>(db, cache),
        IRoomRepository
{
    public async Task<Result<IReadOnlyList<Room>>> GetAllByHotelAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<IReadOnlyList<Room>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync(ct)
        );

    public async Task<bool> NumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct)
        => await db.Rooms
            .AnyAsync(r => r.HotelId == hotelId
                           && r.Number == roomNumber, ct);

    public async Task<Result<Guid>> GetHotelIdAsync(
        Guid roomId,
        CancellationToken ct)
    {
        var result = await db.Rooms
            .Where(r => r.Id == roomId)
            .Select(r => (Guid?)r.HotelId)
            .FirstOrDefaultAsync(ct);

        if (result is null)
            return Result<Guid>.Failure(new Error($"room with ID {roomId} not found", ErrorCode.NotFound),
                ResultCode.NotFound);
        return Result<Guid>.Success(result.Value);
    }

    public async Task<Result<Guid?>> GetManagerIdAsync(
        Guid roomId,
        CancellationToken ct)
    {
        var result = await db.Rooms
            .Where(r => r.Id == roomId)
            .Select(r => new
            {
                ManagerId = r.Hotel.Manager == null
                    ? (Guid?)null
                    : r.Hotel.Manager.Id
            })
            .FirstOrDefaultAsync(ct);

        if (result is null)
            return Result<Guid?>.Failure(new Error($"room with ID {roomId} not found", ErrorCode.NotFound),
                ResultCode.NotFound);
        return Result<Guid?>.Success(result.ManagerId);
    }

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .Select(r => r.Id)
                .ToListAsync(ct)
        );

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(
            await db.Rooms
                .Where(r => hotelIds.Contains(r.HotelId))
                .Select(r => r.Id)
                .ToListAsync(ct)
        );

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByManagerAsync(
        Guid managerId,
        CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(await db.Rooms
            .Where(r => r.Hotel.Manager != null && r.Hotel.Manager.Id == managerId)
            .Select(r => r.Id)
            .ToListAsync(ct));
    
    // same as ManagerRepository.ManagesRoomAsync(Guid managerId, Guid roomId, CancellationToken ct)
    public async Task<bool> IsManagedByManagerAsync(
            Guid roomId,
            Guid managerId,
            CancellationToken ct)
        => await db.Rooms
            .AnyAsync(r => r.Id == roomId
                           && r.Hotel.Manager != null
                           && r.Hotel.Manager.Id == managerId, ct);

    public async Task<bool> BelongsToHotelAsync(
        Guid roomId,
        Guid hotelId,
        CancellationToken ct)
        => await db.Rooms
            .AnyAsync(r => r.Id == roomId
                           && r.HotelId == hotelId, ct);
}