namespace Domain.Models;

public class Manager
    : User
{
    public Guid? HotelId { get; set; }
    public Hotel? Hotel { get; set; }
}