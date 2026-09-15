using Domain.Interfaces;
using SharedKernel.Enums;

namespace Domain.Models;

public class Reservation
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required DateTimeOffset CheckInDate { get; set; }
    public required DateTimeOffset CheckOutDate { get; set; }
    public decimal TotalPrice { get; private set; }
    public ReservationStatus Status { get; set; }

    public required Guid GuestId { get; set; }
    public Guest Guest { get; set; } = null!;
    public required Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public decimal SetTotalPrice(decimal pricePerNight)
    {
        TotalPrice=(CheckOutDate.Date - CheckInDate.Date).Days * pricePerNight;
        return TotalPrice;
    }
}