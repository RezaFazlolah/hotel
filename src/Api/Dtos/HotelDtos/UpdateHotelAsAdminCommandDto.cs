using Api.Dtos.RoomDtos;

namespace Api.Dtos.HotelDtos;

public record UpdateHotelAsAdminCommandDto
    : UpdateHotelAsManagerCommandDto
{
    public required decimal Rating { get; init; }
    public required Guid? ManagerId { get; init; } 
}