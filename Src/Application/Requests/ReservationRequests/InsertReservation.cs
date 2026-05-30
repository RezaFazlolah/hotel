using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Requests.ReservationRequests;

public class InsertReservation : IRequest<Result<Reservation>>
{
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
}