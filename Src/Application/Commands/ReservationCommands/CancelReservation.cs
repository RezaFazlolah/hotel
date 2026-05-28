using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Commands.ReservationCommands;

public class CancelReservation : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; init; }
}