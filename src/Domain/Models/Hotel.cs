using Domain.Interfaces;
using SharedKernel.Common;

namespace Domain.Models;

public class Hotel
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Rating { get; set; }

    public Manager? Manager { get; set; }
    public ICollection<Room> Rooms { get; init; } = [];

    public Result AssignManager(Manager? manager)
    {
        if (manager?.HotelId is null)
        {
            Manager = manager;
            return Result.Success();
        }

        return manager.HotelId == Id
            ? Result.Success()
            : Result.Failure(new Error($"manager {manager.Id} already manages another hotel"));
    }
}