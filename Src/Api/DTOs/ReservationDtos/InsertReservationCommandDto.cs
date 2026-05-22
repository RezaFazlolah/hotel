namespace Api.DTOs.ReservationDtos;

public class InsertReservationCommandDto
{
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
}