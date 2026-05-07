using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class UpdateReservationCommand : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; set; }
    public Guid GuestId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public Guid RoomId { get; set; }
}