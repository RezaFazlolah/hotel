using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record UpdateReservationCommand(Guid ReservationId, DateTimeOffset CheckInDate, DateTimeOffset CheckOutDate)
    : IRequest<Result<ReservationDto>>;