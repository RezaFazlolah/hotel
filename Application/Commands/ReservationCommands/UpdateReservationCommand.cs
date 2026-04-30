using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class UpdateReservationCommand : IRequest<Result<Reservation>>
{
    public Guid GuestId { get; set; }
    public Guid ReservationId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    // foreign key
    public Guid RoomId { get; set; }
}