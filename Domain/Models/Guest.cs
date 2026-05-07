namespace Domain.Models;

public class Guest : User
{
    public ICollection<Reservation> Reservations { get; set; } = [];
}