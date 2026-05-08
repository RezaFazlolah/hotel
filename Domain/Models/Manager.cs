namespace Domain.Models;

public class Manager 
    : User
{
    public required Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
}