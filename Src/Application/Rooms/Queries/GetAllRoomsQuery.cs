using Application.Common;
using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Rooms.Queries;

public record GetAllRoomsQuery
    : IRequest<Result<PagedResult<RoomDto>>>
{
    public required PaginationParameters PaginationParameters { get; init; }
}