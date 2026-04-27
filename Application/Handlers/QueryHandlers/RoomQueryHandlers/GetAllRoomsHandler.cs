using Application.Queries.RoomQueries;
using Application.Result;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers.RoomQueryHandlers;

public class GetAllRoomsHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetAllRoomsQuery, Result<ICollection<Room>>>
{
    public async Task<Result<ICollection<Room>>> Handle(GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllAsync(
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