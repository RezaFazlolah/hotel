namespace Api.DTOs.ReservationDtos;

public class InsertReservationCommandDto
{
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public Guid RoomId { get; set; }
}