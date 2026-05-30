using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.ReservationRequests;

public class GetReservationById
    : IRequest<Result<Reservation>>
{
    public Guid UserId { get; init; }
    public required Guid ReservationId { get; init; }
}