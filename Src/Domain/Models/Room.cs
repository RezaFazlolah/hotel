using Domain.Enums;

namespace Domain.Models;

public class Room
    : IBaseModel<Guid>
{
    public Guid Id { get; set; }
    public required int Number { get; set; }
    public RoomType Type { get; set; }

    public decimal PricePerNight { get; set; }

    // foreign key
    public required Guid HotelId { get; set; }

    // navigation property
    public Hotel? Hotel { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];
}