using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.ReservationQueries;

public class GetAllReservationsQuery
    : IRequest<Result<ICollection<Reservation>>>
{
    public Guid GuestId { get; init; }
}