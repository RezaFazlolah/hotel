using Domain.Models;

namespace Application.Hotels.Filters;

public static class HotelFilterExtensions
{
    extension(IQueryable<Hotel> query)
    {
        public IQueryable<Hotel> ApplyFilter(HotelFilterParameters? filterParameters)
        {
            if(filterParameters is null)
                return query;
            
            if (filterParameters.Name is not null)
                query = query.Where(h => h.Name.Contains(filterParameters.Name));
            if (filterParameters.Address is not null)
                query = query.Where(h => h.Address.Contains(filterParameters.Address));
            if (filterParameters.MinRating.HasValue)
                query = query.Where(h => filterParameters.MinRating.Value <= h.Rating);
            if (filterParameters.MaxRating.HasValue)
                query = query.Where(h => h.Rating <= filterParameters.MaxRating.Value);

            return query;
        }
    }
}