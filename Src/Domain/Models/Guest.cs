namespace Domain.Models;

public class Guest
    : User
{
    public IReadOnlyList<Reservation> Reservations { get; set; } = [];
}