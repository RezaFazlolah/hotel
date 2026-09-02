using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Commands;

public record UpdateRoomAsAdminCommand
    : UpdateRoomBaseCommand
{
    public required Guid HotelId { get; init; }
}