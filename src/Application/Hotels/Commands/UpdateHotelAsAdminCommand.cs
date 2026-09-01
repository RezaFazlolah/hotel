namespace Application.Hotels.Commands;

public record UpdateHotelAsAdminCommand
    : UpdateHotelCommandBase
{
    public required decimal Rating { get; init; }
}