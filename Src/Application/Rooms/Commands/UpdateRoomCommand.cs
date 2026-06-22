using Application.Dtos.RoomDtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public class UpdateRoomCommand
    : IRequest<Result<RoomDto>>
{
    public required Guid Id { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public Guid? HotelId { get; set; }
}