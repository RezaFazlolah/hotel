namespace Api.DTOs.HotelDtos;

public class UpdateHotelDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public float Rating { get; set; }
}
