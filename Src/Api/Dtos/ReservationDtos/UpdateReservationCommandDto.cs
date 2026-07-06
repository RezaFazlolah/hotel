namespace Api.Dtos.ReservationDtos;

public class UpdateReservationCommandDto
{
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
}