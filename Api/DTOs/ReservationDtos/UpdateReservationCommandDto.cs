namespace Api.DTOs.ReservationDtos;

public class UpdateReservationCommandDto
{
    public Guid ReservationId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    // foreign key
    public Guid RoomId { get; set; }
}