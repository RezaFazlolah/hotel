using Domain.Enums;

namespace Domain.Models;

public class Room : IBaseModel<Guid>
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Type { get; set; }
    public decimal PricePerNight { get; set; }
    // foreign key
    public Guid HotelId { get; set; }
    // navigation property
    public required Hotel Hotel { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];
}