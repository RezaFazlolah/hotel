using Domain.Models;

namespace Application.Reservations.Filters;

public static class ReservationFilterExtensions
{
    extension(IQueryable<Reservation> query)
    {
        public IQueryable<Reservation> ApplyFilter(ReservationFilterParameters? reservationFilterParameters)
        {
            if(reservationFilterParameters is null)
                return query;
            
            if (reservationFilterParameters.MinCheckInDate.HasValue)
                query = query.Where(r => reservationFilterParameters.MinCheckInDate.Value <= r.CheckInDate);
            if (reservationFilterParameters.MaxCheckInDate.HasValue)
                query = query.Where(r => r.CheckInDate <= reservationFilterParameters.MaxCheckInDate.Value);

            if (reservationFilterParameters.MinCheckOutDate.HasValue)
                query = query.Where(r => reservationFilterParameters.MinCheckOutDate.Value <= r.CheckOutDate);
            if (reservationFilterParameters.MaxCheckOutDate.HasValue)
                query = query.Where(r => r.CheckOutDate <= reservationFilterParameters.MaxCheckOutDate.Value);

            if (reservationFilterParameters.MinTotalPrice.HasValue)
                query = query.Where(r => reservationFilterParameters.MinTotalPrice.Value <= r.TotalPrice);
            if (reservationFilterParameters.MaxTotalPrice.HasValue)
                query = query.Where(r => r.TotalPrice <= reservationFilterParameters.MaxTotalPrice.Value);

            if (reservationFilterParameters.Status.HasValue)
                query = query.Where(r => r.Status == reservationFilterParameters.Status.Value);

            return query;
        }
    }
}