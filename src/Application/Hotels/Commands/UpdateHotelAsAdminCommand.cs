namespace Application.Hotels.Commands;

public record UpdateHotelAsAdminCommand
    : UpdateHotelBaseCommand
{
    public required decimal Rating { get; init; }
}