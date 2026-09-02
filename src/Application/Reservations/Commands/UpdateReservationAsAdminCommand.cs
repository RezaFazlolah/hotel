using SharedKernel.Enums;

namespace Application.Reservations.Commands;

public record UpdateReservationAsAdminCommand
    : UpdateReservationBaseCommand
{
    public required Guid RoomId { get; set; }
    public required ReservationStatus Status { get; init; }
}