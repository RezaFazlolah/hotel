using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext db)
    : BaseRepository<Guid, Room>(db),
        IRoomRepository
{
    public override async Task<Result<Room>> InsertAsync(
        Room room,
        CancellationToken ct)
        => await RoomNumberExistsAsync(room.HotelId, room.Number, ct)
            ? Result<Room>.Failure(new Error($"room number {room.Number} already exists"))
            : await base.InsertAsync(room, ct);

    public override async Task<Result<Room>> UpdateAsync(
        Room room,
        CancellationToken ct)
    {
        var existingRoomResult = await GetByIdAsync(room.Id, ct);
        if (!existingRoomResult.Succeeded)
            return Result<Room>.Failure(existingRoomResult.Errors.Prepend(new Error($"update room {room.Id} failed")));
        var existingRoom = existingRoomResult.Value;

        if (room.Number != existingRoom.Number)
        {
            if (await RoomNumberExistsAsync(room.HotelId, room.Number, ct))
                return Result<Room>.Failure(
                    new Error($"update room {room.Id} failed. room number {room.Number} already exists"));
        }

        return await base.UpdateAsync(room, ct);
    }

    public async Task<Result<IReadOnlyList<Room>>> GetAllByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<IReadOnlyList<Room>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync(ct)
        );

    public async Task<bool> RoomNumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct)
        => await db.Rooms
            .AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber, ct);

    // Performance: fetch only HotelId column instead of fetching all columns and returning only HotelId
    public async Task<Result<Guid>> GetHotelIdAsync(
        Guid roomId,
        CancellationToken ct)
    {
        var roomResult = await GetByIdAsync(roomId, ct);
        if (!roomResult.Succeeded)
            return Result<Guid>.Failure(roomResult.Errors);
        var room = roomResult.Value;

        return Result<Guid>.Success(room.HotelId);
    }

    public async Task<Result<Guid?>> GetManagerIdAsync(Guid roomId, CancellationToken ct)
    {
        var room = await db.Rooms
            .Include(r => r.Hotel)
            .ThenInclude(h => h.Manager)
            .FirstOrDefaultAsync(r => r.Id == roomId, ct);

        return room is null
            ? Result<Guid?>.Failure(new Error($"room {roomId} not found"))
            : Result<Guid?>.Success(room.Hotel.Manager?.Id);
    }

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(
            await db.Rooms
                .Where(r => r.HotelId == hotelId)
                .Select(r => r.Id)
                .ToListAsync(ct)
        );

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByHotelIdsAsync(
        IEnumerable<Guid> hotelIds,
        CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(
            await db.Rooms
                .Where(r => hotelIds.Contains(r.HotelId))
                .Select(r => r.Id)
                .ToListAsync(ct)
        );

    public async Task<Result<IReadOnlyList<Guid>>> GetAllIdsByManagerIdAsync(Guid managerId, CancellationToken ct)
        => Result<IReadOnlyList<Guid>>.Success(await db.Rooms
            .Where(r => r.Hotel.Manager != null && r.Hotel.Manager.Id == managerId)
            .Select(r => r.Id)
            .ToListAsync(ct));

    public async Task<bool> IsRoomManagedByManagerAsync(Guid roomId, Guid managerId, CancellationToken ct)
    {
        var managerIdResult = await GetManagerIdAsync(managerId, ct);
        return !managerIdResult.Succeeded || managerIdResult.Value is null
            ? false
            : managerIdResult.Value.Equals(roomId);
    }
}