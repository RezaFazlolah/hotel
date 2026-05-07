using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class InsertReservationCommand : IRequest<Result<Reservation>>
{
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
}