namespace Api.Dtos.ReservationDtos;

public record UpdateReservationAsManagerCommandDto
    : UpdateReservationAsGuestCommandDto
{
    public required Guid RoomId { get; set; }
}