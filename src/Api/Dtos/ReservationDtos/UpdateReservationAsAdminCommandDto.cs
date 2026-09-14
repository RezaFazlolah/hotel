using SharedKernel.Enums;

namespace Api.Dtos.ReservationDtos;

public record UpdateReservationAsAdminCommandDto
    : UpdateReservationAsManagerCommandDto
{
    public required ReservationStatus Status { get; init; }
}