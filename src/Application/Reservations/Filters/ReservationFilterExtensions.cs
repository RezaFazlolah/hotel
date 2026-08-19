using Domain.Models;

namespace Application.Reservations.Filters;

public static class ReservationFilterExtensions
{
    extension(IQueryable<Reservation> query)
    {
        public IQueryable<Reservation> ApplyFilter(ReservationFilterParameters? filterParameters)
        {
            if(filterParameters is null)
                return query;
            
            if (filterParameters.MinCheckInDate.HasValue)
                query = query.Where(r => filterParameters.MinCheckInDate.Value <= r.CheckInDate);
            if (filterParameters.MaxCheckInDate.HasValue)
                query = query.Where(r => r.CheckInDate <= filterParameters.MaxCheckInDate.Value);

            if (filterParameters.MinCheckOutDate.HasValue)
                query = query.Where(r => filterParameters.MinCheckOutDate.Value <= r.CheckOutDate);
            if (filterParameters.MaxCheckOutDate.HasValue)
                query = query.Where(r => r.CheckOutDate <= filterParameters.MaxCheckOutDate.Value);

            if (filterParameters.MinTotalPrice.HasValue)
                query = query.Where(r => filterParameters.MinTotalPrice.Value <= r.TotalPrice);
            if (filterParameters.MaxTotalPrice.HasValue)
                query = query.Where(r => r.TotalPrice <= filterParameters.MaxTotalPrice.Value);

            if (filterParameters.Status.HasValue)
                query = query.Where(r => r.Status == filterParameters.Status.Value);

            return query;
        }
    }
}