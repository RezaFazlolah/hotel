using Application.Result;
using Domain.Models;
using MediatR;

namespace Application.Commands.ReservationCommands;

public class InsertReservationCommand : IRequest<Result<Reservation>>
{
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
}