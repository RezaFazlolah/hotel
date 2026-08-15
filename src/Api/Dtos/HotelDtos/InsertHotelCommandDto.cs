namespace Api.Dtos.HotelDtos;

public record InsertHotelCommandDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public float Rating { get; init; }
    public Guid? ManagerId { get; init; }
    public IEnumerable<Guid> RoomIds { get; init; } = [];
}