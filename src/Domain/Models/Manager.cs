namespace Domain.Models;

public class Manager
    : User
{
    public Hotel? Hotel { get; set; }
}