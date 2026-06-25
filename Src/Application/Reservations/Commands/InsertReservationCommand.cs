using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record InsertReservationCommand(
    Guid GuestId,
    Guid RoomId,
    DateTimeOffset CheckInDate,
    DateTimeOffset CheckOutDate)
    : IRequest<Result<ReservationDto>>;