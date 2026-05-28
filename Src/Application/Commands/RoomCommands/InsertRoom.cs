using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Commands.RoomCommands;

public class InsertRoom : IRequest<Result<Room>>
{
    public required int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public required Guid HotelId { get; set; }
}