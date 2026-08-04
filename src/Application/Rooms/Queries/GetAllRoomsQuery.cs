using Application.Rooms.Dtos;
using Application.Rooms.Filters;
using Application.Rooms.Sorts;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Rooms.Queries;

public record GetAllRoomsQuery
    : IRequest<Result<PagedResult<RoomDto>>>
{
    public required PaginationParameters PaginationParameters { get; init; }
    public required RoomSortParameters RoomSortParameters { get; init; }
    public required RoomFilterParameters? RoomFilterParameters { get; init; }
}