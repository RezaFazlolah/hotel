using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Commands.ReservationCommands;

public class UpdateReservation : IRequest<Result<Reservation>>
{
    public required Guid ReservationId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
}