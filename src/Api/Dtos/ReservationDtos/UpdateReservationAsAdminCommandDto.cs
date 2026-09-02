using SharedKernel.Enums;

namespace Api.Dtos.ReservationDtos;

public record UpdateReservationAsAdminCommandDto
    : UpdateReservationBaseCommandDto
{
    public required Guid RoomId { get; set; }
    public required ReservationStatus Status { get; init; }
}