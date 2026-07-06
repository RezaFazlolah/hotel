using Domain.Models;

namespace Application.Rooms.Filters;

public static class RoomFilterExtensions
{
    public static IQueryable<Room> ApplyFilter(
        this IQueryable<Room> query,
        RoomFilterParameters roomFilterParameters)
    {
        if (roomFilterParameters.MinNumber.HasValue)
            query = query.Where(r => roomFilterParameters.MinNumber.Value <= r.Number);
        if (roomFilterParameters.MaxNumber.HasValue)
            query = query.Where(r => r.Number <= roomFilterParameters.MaxNumber.Value);
        
        if(roomFilterParameters.Type.HasValue)
            query = query.Where(r => r.Type == roomFilterParameters.Type);

        if (roomFilterParameters.MinPricePerNight.HasValue)
            query = query.Where(r => roomFilterParameters.MinPricePerNight.Value <= r.PricePerNight);
        if (roomFilterParameters.MaxPricePerNight.HasValue)
            query = query.Where(r => r.PricePerNight <= roomFilterParameters.MaxPricePerNight.Value);
        
        return query;
    }
}