namespace Application.Reservations.Commands;

public record UpdateReservationAsManagerCommand
    : UpdateReservationAsGuestCommand
{
    public required Guid RoomId { get; set; }
}