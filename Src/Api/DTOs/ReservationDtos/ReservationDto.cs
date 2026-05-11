using System;
using Api.DTOs.RoomDtos;

namespace Api.DTOs.ReservationDtos;

public class ReservationDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CheckInDate { get; set; }
    public DateTimeOffset CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public required RoomDto RoomDto { get; set; }
}
