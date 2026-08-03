namespace Api.Dtos.HotelDtos;

public class InsertHotelCommandDto
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public float Rating { get; set; }
    public Guid ManagerId { get; set; }
}
