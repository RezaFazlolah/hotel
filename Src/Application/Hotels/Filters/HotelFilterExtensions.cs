using Domain.Models;

namespace Application.Hotels.Filters;

public static class HotelFilterExtensions
{
    public static IQueryable<Hotel> ApplyFilter(
        this IQueryable<Hotel> query,
        HotelFilterParameters hotelFilterParameters)
    {
        if (hotelFilterParameters.Name is not null)
            query = query.Where(h => h.Name.Contains(hotelFilterParameters.Name));
        if (hotelFilterParameters.Address is not null)
            query = query.Where(h => h.Address.Contains(hotelFilterParameters.Address));
        if (hotelFilterParameters.MinRating.HasValue)
            query = query.Where(h => hotelFilterParameters.MinRating.Value <= h.Rating);
        if (hotelFilterParameters.MaxRating.HasValue)
            query = query.Where(h => h.Rating <= hotelFilterParameters.MaxRating.Value);

        return query;
    }
}