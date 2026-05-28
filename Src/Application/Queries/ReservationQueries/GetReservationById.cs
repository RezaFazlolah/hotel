using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Queries.ReservationQueries;

public class GetReservationById
    : IRequest<Result<Reservation>>
{
    public Guid UserId { get; init; }
    public required Guid ReservationId { get; init; }
}