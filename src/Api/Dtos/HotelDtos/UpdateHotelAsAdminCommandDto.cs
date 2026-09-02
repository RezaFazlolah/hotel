using Api.Dtos.RoomDtos;

namespace Api.Dtos.HotelDtos;

public record UpdateHotelAsAdminCommandDto
    : UpdateHotelBaseCommandDto
{
    public required decimal Rating { get; init; }
}