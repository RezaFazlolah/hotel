using Api.Dtos.RoomDtos;

namespace Api.Dtos.HotelDtos;

public record UpdateHotelAsAdminCommandDto
    : UpdateHotelCommandBaseDto
{
    public required decimal Rating { get; init; }
}