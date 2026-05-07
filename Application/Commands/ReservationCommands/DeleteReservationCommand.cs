using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class DeleteReservationCommand : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; init; }
    public Guid GuestId { get; init; }
}