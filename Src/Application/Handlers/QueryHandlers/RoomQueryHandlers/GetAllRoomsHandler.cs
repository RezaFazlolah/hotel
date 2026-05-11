using Application.Interfaces.ServiceInterfaces;
using Application.Queries.RoomQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.QueryHandlers.RoomQueryHandlers;

public class GetAllRoomsHandler(IRoomService roomService)
    : IRequestHandler<GetAllRoomsQuery, Result<ICollection<Room>>>
{
    public async Task<Result<ICollection<Room>>> Handle(GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await roomService.GetAllAsync(
            cancellationToken,
            filterOn: request.FilterOn,
            filterQuery: request.FilterQuery,
            orderBy: request.OrderBy,
            isAscending: request.IsAscending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        return Result<ICollection<Room>>.Success(rooms);
    }
}