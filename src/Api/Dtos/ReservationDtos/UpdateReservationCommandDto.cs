namespace Api.Dtos.ReservationDtos;

public record UpdateReservationCommandDto
{
    public DateTimeOffset? CheckInDate { get; init; }
    public DateTimeOffset? CheckOutDate { get; init; }
}