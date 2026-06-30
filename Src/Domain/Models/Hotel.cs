using Domain.Interface;

namespace Domain.Models;

public class Hotel
    : IEntity<Guid>
{
    public Guid Id { get; set; } = new();
    public required string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public decimal Rating { get; set; }

    // navigation property
    public IReadOnlyList<Room> Rooms { get; set; } = [];
    public IReadOnlyList<Manager> Managers { get; set; } = [];
}