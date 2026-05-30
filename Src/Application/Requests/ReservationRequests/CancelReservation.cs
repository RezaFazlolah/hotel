using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.ReservationRequests;

public class CancelReservation : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; init; }
}