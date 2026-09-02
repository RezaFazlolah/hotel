using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Rooms.Commands;

public record UpdateRoomAsManagerCommand
    : UpdateRoomBaseCommand
{
}