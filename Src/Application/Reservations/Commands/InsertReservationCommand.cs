using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record InsertReservationCommand
    : IRequest<Result<ReservationDto>>
{
        public required Guid GuestId { get; init; }
        public required Guid RoomId { get; init; }
        public required DateTimeOffset CheckInDate { get; init; }
        public required DateTimeOffset CheckOutDate { get; init; }
}