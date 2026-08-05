using Domain.Models;

namespace Application.Reservations.Sorts;

public static class ReservationExtensions
{
    extension(IQueryable<Reservation> query)
    {
        public IQueryable<Reservation> ApplySort(ReservationSortParameters? reservationSortParameters)
        {
            if (reservationSortParameters is null)
                return query.OrderBy(r => r.Id);

            return reservationSortParameters.SortBy switch
            {
                ReservationSortBy.CheckInDate => reservationSortParameters.IsAscending
                    ? query.OrderBy(r => r.CheckInDate)
                    : query.OrderByDescending(r => r.CheckInDate),
                ReservationSortBy.CheckOutDate => reservationSortParameters.IsAscending
                    ? query.OrderBy(r => r.CheckOutDate)
                    : query.OrderByDescending(r => r.CheckOutDate),
                ReservationSortBy.TotalPrice => reservationSortParameters.IsAscending
                    ? query.OrderBy(r => r.TotalPrice)
                    : query.OrderByDescending(r => r.TotalPrice),
                ReservationSortBy.Status => reservationSortParameters.IsAscending
                    ? query.OrderBy(r => r.Status)
                    : query.OrderByDescending(r => r.Status),
                _ => reservationSortParameters.IsAscending
                    ? query.OrderBy(r => r.Id)
                    : query.OrderByDescending(r => r.Id)
            };
        }
    }
}