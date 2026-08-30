using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record UpdateReservationCommand
    : IRequest<Result<ReservationDto>>
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CheckInDate { get; init; }
    public required DateTimeOffset CheckOutDate { get; init; }
}