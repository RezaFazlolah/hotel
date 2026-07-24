using Domain.Interfaces;

namespace Domain.Models;

public class Hotel
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Rating { get; set; }

    public IReadOnlyList<Room> Rooms { get; set; } = [];
    public Manager Manager { get; set; } = null!;
}