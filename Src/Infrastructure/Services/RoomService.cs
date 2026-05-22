using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Infrastructure.Services;

public class RoomService(AppDbContext context, IHotelService hotelService)
    : BaseService<Guid, Room>(context), IRoomService
{
    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid roomId, CancellationToken ct)
        => await GetReservationsAsync([roomId], ct);

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> roomsId, CancellationToken ct)
        // implement with ReservationService's GetReservations() with proper filter instead of this
        => Result<ICollection<Reservation>>.Success(await context.Reservations.Where(r => roomsId.Contains(r.RoomId)).ToListAsync(ct));

    public async Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate, CancellationToken ct)
        // implement with ReservationService's Exists() with proper filter instead of this
        => Result<bool>.Success(await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate || checkOutDate < r.CheckInDate) &&
            r.Status != ReservationStatus.Cancelled, ct));

    public async Task<Result<bool>> IsReservedAsync(Guid roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate, Guid guestId,
        CancellationToken ct)
        // implement with ReservationService's Exists() with proper filter instead of this
        => Result<bool>.Success(await context.Reservations.AnyAsync(r =>
            r.RoomId == roomId && !(r.CheckOutDate < checkInDate ||
                                    checkOutDate < r.CheckInDate && r.Status != ReservationStatus.Cancelled) &&
            r.GuestId != guestId, ct));

    public override async Task<Result<Room>> InsertAsync(Room room, CancellationToken cancellationToken)
        => (await hotelService.RoomNumberExistsAsync(room.Number, room.HotelId, cancellationToken)).Value
            ? Result<Room>.Failure(new Error($"room number {room.Number} already exists"))
            : await base.InsertAsync(room, cancellationToken);

    public override async Task<Result<Room>> UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        if ((await hotelService.RoomNumberExistsAsync(room.Number, room.HotelId, cancellationToken)).Value)
            return Result<Room>.Failure(new Error($"room number {room.Number} already exists"));

        return await base.UpdateAsync(room, cancellationToken);
    }

    protected override IQueryable<Room> CustomContext()
        => context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.Reservations);

    protected override IQueryable<Room> CustomFilter(IQueryable<Room> query, string? filterOn, string? filterQuery)
    {
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            // filter by room number
            if (filterOn.Equals("Number", StringComparison.OrdinalIgnoreCase)) // case-insensitive
                query = query.Where(r => r.Number.ToString().Contains(filterQuery));

            // filter by room type
            if (filterOn.Equals("Type", StringComparison.OrdinalIgnoreCase))
                query = query.Where(r => r.Type.ToString() == filterQuery);
        }

        return query;
    }

    protected override IQueryable<Room> CustomSort(IQueryable<Room> query, string? orderBy, bool isAscending)
    {
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            // sort by room number
            if (orderBy.Equals("Number", StringComparison.OrdinalIgnoreCase))
                query = isAscending
                    ? query.OrderBy(r => r.Number)
                    : query.OrderByDescending(r => r.Number);

            // sort by room PricePerNight
            if (orderBy.Equals("PricePerNight", StringComparison.OrdinalIgnoreCase))
                query = isAscending
                    ? query.OrderBy(r => r.PricePerNight)
                    : query.OrderByDescending(r => r.PricePerNight);
        }

        return query;
    }
}