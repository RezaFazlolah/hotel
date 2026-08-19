using Domain.Models;

namespace Application.Rooms.Filters;

public static class RoomFilterExtensions
{
    public static IQueryable<Room> ApplyFilter(
        this IQueryable<Room> query,
        RoomFilterParameters? filterParameters)
    {
        if(filterParameters is null)
            return query;
        
        if (filterParameters.MinNumber.HasValue)
            query = query.Where(r => filterParameters.MinNumber.Value <= r.Number);
        if (filterParameters.MaxNumber.HasValue)
            query = query.Where(r => r.Number <= filterParameters.MaxNumber.Value);
        
        if(filterParameters.Type.HasValue)
            query = query.Where(r => r.Type == filterParameters.Type);

        if (filterParameters.MinPricePerNight.HasValue)
            query = query.Where(r => filterParameters.MinPricePerNight.Value <= r.PricePerNight);
        if (filterParameters.MaxPricePerNight.HasValue)
            query = query.Where(r => r.PricePerNight <= filterParameters.MaxPricePerNight.Value);
        
        return query;
    }
}