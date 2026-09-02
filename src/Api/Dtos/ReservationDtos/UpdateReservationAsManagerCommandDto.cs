namespace Api.Dtos.ReservationDtos;

public record UpdateReservationAsManagerCommandDto
    : UpdateReservationBaseCommandDto
{
    public required Guid RoomId { get; set; }
}