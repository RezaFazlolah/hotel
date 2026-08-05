using Domain.Models;

namespace Application.Rooms.Sorts;

public static class RoomSortExtensions
{
    extension(IQueryable<Room> query)
    {
        public IQueryable<Room> ApplySort(RoomSortParameters? roomSortParameters)
        {
            if (roomSortParameters is null)
                return query.OrderBy(r => r.Id);

            return roomSortParameters.SortBy switch
            {
                RoomSortBy.Number => roomSortParameters.IsAscending
                    ? query.OrderBy(r => r.Number)
                    : query.OrderByDescending(r => r.Number),
                RoomSortBy.Type => roomSortParameters.IsAscending
                    ? query.OrderBy(r => r.Type)
                    : query.OrderByDescending(r => r.Type),
                RoomSortBy.PricePerNight => roomSortParameters.IsAscending
                    ? query.OrderBy(r => r.PricePerNight)
                    : query.OrderByDescending(r => r.PricePerNight),
                _ => roomSortParameters.IsAscending
                    ? query.OrderBy(r => r.Id)
                    : query.OrderByDescending(r => r.Id)
            };
        }
    }
}