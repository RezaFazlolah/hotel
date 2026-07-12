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
        => (await RoomNumberExistsAsync(room.HotelId, room.Number, ct)).Value
            ? Result<Room>.Failure(new Error($"room number {room.Number} already exists"))
            : await base.InsertAsync(room, ct);

    public override async Task<Result<Room>> UpdateAsync(
        Room room,
        CancellationToken ct)
    {
        var existingRoomResult = await GetByIdAsync(room.Id, ct);
        if (!existingRoomResult.Succeeded)
            return Result<Room>.Failure(existingRoomResult.Errors.Prepend(new Error($"update room {room.Id} failed.")));
        var existingRoom = existingRoomResult.Value;

        if (room.Number != existingRoom.Number)
        {
            var roomNumberExistsResult =
                await RoomNumberExistsAsync(room.HotelId, room.Number, ct);
            if (!roomNumberExistsResult.Succeeded)
                return Result<Room>.Failure(
                    roomNumberExistsResult.Errors.Prepend(new Error($"update room {room.Id} failed.")));
            var roomNumberExists = roomNumberExistsResult.Value;
            if (roomNumberExists)
                return Result<Room>.Failure(
                    new Error($"update room {room.Id} failed. room number {room.Number} already exists."));
        }

        return await base.UpdateAsync(room, ct);
    }

    protected override IQueryable<Room> CustomContext()
        => db.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.Reservations);

    public async Task<Result<IReadOnlyList<Room>>> GetAllByHotelIdAsync(
        Guid hotelId,
        CancellationToken ct)
        => Result<IReadOnlyList<Room>>.Success(
            await CustomContext()
                .Where(r => r.HotelId == hotelId)
                .ToListAsync(ct)
        );

    public async Task<Result<bool>> RoomNumberExistsAsync(
        Guid hotelId,
        int roomNumber,
        CancellationToken ct)
    {
        if (!await ExistsAsync(hotelId, ct))
            return Result<bool>.Failure(new Error($"hotel {hotelId} not found."));

        var roomExists = await db.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber, ct);
        return Result<bool>.Success(roomExists);
    }

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
}