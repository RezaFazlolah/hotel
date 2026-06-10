using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext context, IHotelRepository hotelRepository)
    : BaseRepository<Guid, Room>(context), IRoomRepository
{
    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid roomId, CancellationToken ct)
        => await GetReservationsAsync([roomId], ct);

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> roomsId,
            CancellationToken ct)
        // implement with ReservationRepository's GetReservations() with proper filter instead of this
        => Result<ICollection<Reservation>>.Success(await context.Reservations.Where(r => roomsId.Contains(r.RoomId))
            .ToListAsync(ct));

    public async Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate,
            DateTimeOffset checkOutDate, CancellationToken ct)
        // implement with ReservationRepository's Exists() with proper filter instead of this
        => Result<bool>.Success(await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate) &&
            r.Status != ReservationStatus.Cancelled, ct));

    public async Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate,
            DateTimeOffset checkOutDate, Guid guestId,
            CancellationToken ct)
        // implement with ReservationRepository's Exists() with proper filter instead of this
        => Result<bool>.Success(await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate ||
                                    checkOutDate < r.CheckInDate && r.Status != ReservationStatus.Cancelled) &&
            r.GuestId != guestId, ct));

    public override async Task<Result<Room>> InsertAsync(Room room, CancellationToken cancellationToken)
        => (await hotelRepository.RoomNumberExistsAsync(room.Number, room.HotelId, cancellationToken)).Value
            ? Result<Room>.Failure(new Error($"room number {room.Number} already exists"))
            : await base.InsertAsync(room, cancellationToken);

    public override async Task<Result<Room>> UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        var existingRoomResult = await GetByIdAsync(room.Id, cancellationToken);
        if (!existingRoomResult.Succeeded)
            return Result<Room>.Failure(existingRoomResult.Errors.Prepend(new Error($"update room {room.Id} failed.")));
        var existingRoom = existingRoomResult.Value;

        if (room.Number != existingRoom.Number)
        {
            var roomNumberExistsResult =
                await hotelRepository.RoomNumberExistsAsync(room.Number, room.HotelId, cancellationToken);
            if (!roomNumberExistsResult.Succeeded)
                return Result<Room>.Failure(
                    roomNumberExistsResult.Errors.Prepend(new Error($"update room {room.Id} failed.")));
            var roomNumberExists = roomNumberExistsResult.Value;
            if (roomNumberExists)
                return Result<Room>.Failure(
                    new Error($"update room {room.Id} failed. room number {room.Number} already exists."));
        }

        return await base.UpdateAsync(room, cancellationToken);
    }

    protected override IQueryable<Room> CustomContext()
        => context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.Reservations);
}