namespace Domain.Models;

public class Hotel
    : IBaseModel<Guid>
{
    public Guid Id { get; set; } = new();
    public required string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public decimal Rating { get; set; }

    // navigation property
    public ICollection<Room> Rooms { get; set; } = [];
    public ICollection<Manager> Managers { get; set; } = [];
}