using Domain.Models;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class HotelService(AppDbContext context)
    : BaseService<Guid, Hotel>(context), IHotelService
{
    protected override IQueryable<Hotel> CustomContext()
    {
        return context.Hotels
            .Include(h => h.Rooms);
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

    public async Task<ICollection<Room>> GetRoomsAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsAsync([hotelId], ct);

    public async Task<ICollection<Room>> GetRoomsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
        => await context.Rooms.Where(r => hotelsId.Contains(r.HotelId)).ToListAsync(ct);

    public async Task<ICollection<Guid>> GetRoomsIdAsync(Guid hotelId, CancellationToken ct)
        => await GetRoomsIdAsync([hotelId], ct);

    public async Task<ICollection<Guid>> GetRoomsIdAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
        => await context.Rooms.Where(r => hotelsId.Contains(r.HotelId)).Select(r => r.Id).ToListAsync(ct);

    public async Task<ICollection<Reservation>> GetReservationsAsync(Guid hotelId, CancellationToken ct)
        => await GetReservationsAsync([hotelId], ct);

    public async Task<ICollection<Reservation>> GetReservationsAsync(IEnumerable<Guid> hotelsId, CancellationToken ct)
    {
        var roomsId = await GetRoomsIdAsync(hotelsId, ct);
        return await context.Reservations.Where(r => roomsId.Contains(r.Id)).ToListAsync(ct);
    }
}