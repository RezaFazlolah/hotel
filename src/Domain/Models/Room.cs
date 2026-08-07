using Domain.Interfaces;
using SharedKernel.Enums;

namespace Domain.Models;

public class Room
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }

    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; } = [];
}