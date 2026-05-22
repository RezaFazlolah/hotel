using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace Infrastructure.Services;

public class HotelService(AppDbContext context)
    : BaseService<Guid, Hotel>(context), IHotelService
{
    public async Task<Result<ICollection<Room>>> GetRoomsAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsAsync([hotelId], ct);

    public async Task<Result<ICollection<Room>>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
    // implement with RoomService's GetRooms() with proper filter instead of this
        => Result<ICollection<Room>>.Success(await context.Rooms.Where(r => hotelsId.Contains(r.HotelId)).ToListAsync(ct));

    public async Task<Result<ICollection<Guid>>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsIdAsync([hotelId], ct);

    public async Task<Result<ICollection<Guid>>> GetRoomsIdAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
    // implement with RoomService's GetRoomsId() with proper filter instead of this
        => Result<ICollection<Guid>>.Success(await context.Rooms.Where(r => hotelsId.Contains(r.HotelId)).Select(r => r.Id).ToListAsync(ct));

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(Guid hotelId, CancellationToken ct)
        => await GetReservationsAsync([hotelId], ct);

    public async Task<Result<ICollection<Reservation>>> GetReservationsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
    // implement with ReservationService's GetReservations() with proper filter instead of this
    {
        var roomsId = (await GetRoomsIdAsync(hotelsId, ct)).Value;
        return Result<ICollection<Reservation>>.Success(await context.Reservations.Where(r => roomsId.Contains(r.Id)).ToListAsync(ct));
    }

    public async Task<Result<bool>> RoomNumberExistsAsync(int roomNumber, Guid hotelId, CancellationToken cancellationToken)
        // implement with RoomService's RoomExists() with proper filter instead of this
        => Result<bool>.Success((await context.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber,
            cancellationToken: cancellationToken)));

    protected override IQueryable<Hotel> CustomContext()
    {
        return context.Hotels
            .Include(h => h.Rooms)
            .Include(h => h.Managers);
    }

    protected override IQueryable<Hotel> CustomFilter(IQueryable<Hotel> query, string? filterOn, string? filterQuery)
    {
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            // filter by hotel name
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) // case-insensitive
            {
                query = query.Where(h => h.Name.ToString().Contains(filterQuery));
            }

            // filter by hotel address
            if (filterOn.Equals("Address", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(h => h.Address.Contains(filterQuery));
            }
        }

        return query;
    }

    protected override IQueryable<Hotel> CustomSort(IQueryable<Hotel> query, string? orderBy, bool isAscending)
    {
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            // sort by hotel name
            if (orderBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                query = isAscending
                    ? query.OrderBy(h => h.Name)
                    : query.OrderByDescending(h => h.Name);

            // sort by hotel address
            if (orderBy.Equals("Address", StringComparison.OrdinalIgnoreCase))
                query = isAscending
                    ? query.OrderBy(h => h.Address)
                    : query.OrderByDescending(h => h.Address);

            // sort by hotel rating
            if (orderBy.Equals("Rating", StringComparison.OrdinalIgnoreCase))
            {
                query = isAscending
                    ? query.OrderBy(h => h.Rating)
                    : query.OrderByDescending(h => h.Rating);
            }
        }

        return query;
    }
}