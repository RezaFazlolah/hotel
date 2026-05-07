using Domain.Enums;

namespace Domain.Models;

public class Reservation : IBaseModel<Guid>
{
    public Guid Id { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; } // calculated property

    public ReservationStatus Status { get; set; }

    // foreign key
    public required Guid GuestId { get; set; }
    public required Guid RoomId { get; set; }

    // navigation property
    public Guest? Guest { get; set; }
    public Room? Room { get; set; }
}