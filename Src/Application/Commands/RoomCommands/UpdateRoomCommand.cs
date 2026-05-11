using Application.Models;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Commands.RoomCommands;

public class UpdateRoomCommand : IRequest<Result<Room>>
{
    public required Guid Id { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid? HotelId { get; set; }
}