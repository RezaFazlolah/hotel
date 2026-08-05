using Domain.Models;

namespace Application.Hotels.Sorts;

public static class HotelSortExtensions
{
    extension(IQueryable<Hotel> query)
    {
        public IQueryable<Hotel> ApplySort(HotelSortParameters? hotelSortParameters)
        {
            if (hotelSortParameters is null)
                return query.OrderBy(h => h.Id);
                
            return hotelSortParameters.HotelSortBy switch
            {
                HotelSortBy.Name => hotelSortParameters.IsAscending
                    ? query.OrderBy(h => h.Name)
                    : query.OrderByDescending(h => h.Name),
                HotelSortBy.Address => hotelSortParameters.IsAscending
                    ? query.OrderBy(h => h.Address)
                    : query.OrderByDescending(h => h.Address),
                HotelSortBy.Rating => hotelSortParameters.IsAscending
                    ? query.OrderBy(h => h.Rating)
                    : query.OrderByDescending(h => h.Rating),
                _ => hotelSortParameters.IsAscending
                    ? query.OrderBy(h => h.Id)
                    : query.OrderByDescending(h => h.Id)
            };
        }
    }
}