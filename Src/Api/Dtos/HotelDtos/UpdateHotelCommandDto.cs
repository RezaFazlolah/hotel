namespace Api.Dtos.HotelDtos;

public class UpdateHotelCommandDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
}
