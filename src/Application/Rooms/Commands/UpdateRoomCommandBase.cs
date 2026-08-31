using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public record UpdateRoomCommandBase
{
    public required Guid Id { get; init; }
    public required int Number { get; init; }
    public required RoomType Type { get; init; }
    public required decimal PricePerNight { get; init; }
}