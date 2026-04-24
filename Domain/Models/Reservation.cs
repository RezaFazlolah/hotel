namespace Domain.Models;

public class Reservation : IBaseModel<Guid>
{
    public Guid Id { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    // foreign key
    public Guid GuestId { get; set; }
    public Guid RoomId { get; set; }
    // navigation property
    public required Room Room { get; set; }
}