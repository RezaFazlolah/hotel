namespace Api.DTOs.ReservationDtos;

public class UpdateReservationCommandDto
{
    public required Guid ReservationId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public Guid RoomId { get; set; }
}