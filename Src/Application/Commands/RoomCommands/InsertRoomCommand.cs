using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Commands.RoomCommands;

public class InsertRoomCommand : IRequest<Result<Room>>
{
    public Guid Id { get; set; }
    public required int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public required Guid HotelId { get; set; }
}