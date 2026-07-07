using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record UpdateReservationCommand
    : IRequest<Result<ReservationDto>>
{
    public required Guid Id { get; set; }
    public DateTimeOffset? CheckInDate { get; set; }
    public DateTimeOffset? CheckOutDate { get; set; }
}