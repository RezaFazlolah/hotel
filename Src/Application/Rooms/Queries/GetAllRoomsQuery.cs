using Application.Common;
using Application.Rooms.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Rooms.Queries;

public record GetAllRoomsQuery(PaginationParameters PaginationParameters)
    : IRequest<Result<PagedResult<RoomDto>>>;