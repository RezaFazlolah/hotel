namespace Api.DTOs.ReservationDtos;

public class UpdateReservationDto
{
    public required Guid ReservationId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
}