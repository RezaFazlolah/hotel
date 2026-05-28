namespace Api.DTOs.ReservationDtos;

public class InsertReservationDto
{
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
}