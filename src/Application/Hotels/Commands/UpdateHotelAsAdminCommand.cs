namespace Application.Hotels.Commands;

public record UpdateHotelAsAdminCommand
    : UpdateHotelAsManagerCommand
{
    public required decimal Rating { get; init; }
    public required Guid? ManagerId { get; init; }
}