namespace Application.Auth.Dtos;

public record ManagerDto
    : UserDto
{
    public Guid? HotelId { get; init; }
}