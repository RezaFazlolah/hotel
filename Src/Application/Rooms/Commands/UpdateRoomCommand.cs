using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public record UpdateRoomCommand
    : IRequest<Result<RoomDto>>
{
    public required Guid Id { get; init; }
    public int? Number { get; init; }
    public RoomType? Type { get; init; }
    public decimal? PricePerNight { get; init; }
    public Guid? HotelId { get; init; }
}