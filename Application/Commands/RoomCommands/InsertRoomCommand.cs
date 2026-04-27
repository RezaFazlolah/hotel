using Application.Result;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Commands.RoomCommands;

public class InsertRoomCommand : IRequest<Result<Room>>
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid? HotelId { get; set; }
}