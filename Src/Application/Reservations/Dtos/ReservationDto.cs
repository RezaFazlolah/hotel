using SharedKernel.Enums;

namespace Application.Reservations.Dtos;

public class ReservationDto
{
    public Guid Id { get; set; }
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; }
    
    // public required GuestDto Guest { get; set; }
    // public required RoomDto Room { get; set; }
}