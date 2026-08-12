using Application.Reservations.Dtos;

namespace Application.Users.Dtos;

public class GuestDto
    :  UserDto
{
    public ICollection<ReservationDto> Reservations { get; set; } = [];
}