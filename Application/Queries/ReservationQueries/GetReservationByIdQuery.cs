using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.ReservationQueries;

public class GetReservationByIdQuery
    : IRequest<Result<Reservation>>
{
    public Guid ReservationId { get; init; }
    public Guid GuestId { get; init; }
}