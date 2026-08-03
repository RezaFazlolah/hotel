using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record UpdateReservationCommand
    : IRequest<Result<ReservationDto>>
{
    public required Guid Id { get; init; }
    public DateTimeOffset? CheckInDate { get; init; }
    public DateTimeOffset? CheckOutDate { get; init; }
}