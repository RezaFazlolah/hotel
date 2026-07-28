using Domain.Interfaces;

namespace Domain.Models;

public class Hotel
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Rating { get; set; }

    public Guid ManagerId { get; set; }
    public Manager Manager { get; set; } = null!;
    public ICollection<Room> Rooms { get; } = [];
}