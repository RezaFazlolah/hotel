using Application.Hotels.Dtos;

namespace Application.Users.Dtos;

public class ManagerDto
    : UserDto
{
    public HotelDto? Hotel { get; set; }
}