using Domain.Models;
using Domain.Repositories;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext context) : BaseRepository<Room, Guid>(context), IRoomRepository
{
    public async Task<bool> IsRoomNumberUniqueAsync(Guid roomId, Guid hotelId, int roomNumber, CancellationToken cancellationToken)
    {
        var isRoomNumberUnique =
            !await context.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == roomNumber && r.Id != roomId, cancellationToken);
        return isRoomNumberUnique;
    }

    public override async Task<Room?> InsertAsync(Room entity, CancellationToken cancellationToken)
    {
        var isRoomNumberUnique =
            await IsRoomNumberUniqueAsync(entity.Id, entity.HotelId, entity.Number, cancellationToken);
        if (!isRoomNumberUnique)
            return null;
        return await base.InsertAsync(entity, cancellationToken);
    }

    // roomId MUST NOT be updated
    public override async Task<Room?> UpdateAsync(Room entity, CancellationToken cancellationToken)
    {
        var isRoomNumberUnique =
            await IsRoomNumberUniqueAsync(entity.Id, entity.HotelId, entity.Number, cancellationToken);
        if (!isRoomNumberUnique)
            return null;

        return await base.UpdateAsync(entity, cancellationToken);
    }

    protected override IQueryable<Room> CustomContext()
    {
        return context.Rooms.Include(r => r.Hotel).Include(r => r.Reservations);
    }

    protected override IQueryable<Room> CustomFilter(IQueryable<Room> query, string? filterOn, string? filterQuery)
    {
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            // filter by room number
            if (filterOn.Equals("Number", StringComparison.OrdinalIgnoreCase)) // case-insensitive
            {
                query = query.Where(r => r.Number.ToString().Contains(filterQuery));
            }

            // filter by room type
            if (filterOn.Equals("Type", StringComparison.OrdinalIgnoreCase))
            {
                // convert enum RoomType to string
                if (Enum.TryParse<RoomType>(filterQuery, true, out var type))
                {
                    query = query.Where(r => r.Type == type);
                }
            }
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