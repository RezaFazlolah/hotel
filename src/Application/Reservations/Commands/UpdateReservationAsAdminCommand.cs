using SharedKernel.Enums;

namespace Application.Reservations.Commands;

public record UpdateReservationAsAdminCommand
    : UpdateReservationAsManagerCommand
{
    public required ReservationStatus Status { get; init; }
}