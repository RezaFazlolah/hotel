using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.ReservationQueries;

public class GetReservationByIdQuery
    : IRequest<Result<Reservation>>
{
    public Guid UserId { get; init; }
    public required Guid ReservationId { get; init; }
}