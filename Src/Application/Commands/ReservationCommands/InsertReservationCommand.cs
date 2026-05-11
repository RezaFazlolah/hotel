using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Commands.ReservationCommands;

public class InsertReservationCommand : IRequest<Result<Reservation>>
{
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
}