namespace Application.Reservations.Commands;

public record UpdateReservationAsManagerCommand
    : UpdateReservationBaseCommand
{
    public required Guid RoomId { get; set; }
}