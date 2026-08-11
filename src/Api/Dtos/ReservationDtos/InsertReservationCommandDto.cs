namespace Api.Dtos.ReservationDtos;

public record InsertReservationCommandDto
{
    public required Guid GuestId { get; init; }
    public required Guid RoomId { get; init; }
    public required DateTimeOffset CheckInDate { get; init; }
    public required DateTimeOffset CheckOutDate { get; init; }
}