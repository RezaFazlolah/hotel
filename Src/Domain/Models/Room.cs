using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Enums;

namespace Domain.Models;

public class Room
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }

    public required Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];
}