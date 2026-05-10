using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class CancelReservationCommand : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; init; }
}