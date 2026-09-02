using SharedKernel.Enums;

namespace Application.Reservations.Dtos;

public record ReservationDto
{
    public Guid Id { get; init; }
    public required Guid GuestId { get; init; }
    public required Guid RoomId { get; init; }
    public DateTimeOffset CheckInDate { get; init; }
    public DateTimeOffset CheckOutDate { get; init; }
    public decimal TotalPrice { get; init; }
    public ReservationStatus Status { get; init; }
}