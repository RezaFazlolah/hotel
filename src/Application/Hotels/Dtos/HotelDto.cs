namespace Application.Hotels.Dtos;

public record HotelDto
    : HotelBaseDto
{
    public Guid? ManagerId { get; init; }
}