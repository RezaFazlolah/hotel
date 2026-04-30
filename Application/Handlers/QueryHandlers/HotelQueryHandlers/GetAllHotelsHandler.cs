using Application.Models;
using Application.Queries.HotelQueries;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetAllHotelsHandler(IHotelRepository hotelRepository) : IRequestHandler<GetAllHotelsQuery, Result<ICollection<Hotel>>>
{
    public async Task<Result<ICollection<Hotel>>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await hotelRepository.GetAllAsync(
                cancellationToken,
                filterOn: request.FilterOn,
                filterQuery: request.FilterQuery,
                orderBy: request.OrderBy,
                isAscending: request.IsAscending,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

        return Result<ICollection<Hotel>>.Success(hotels);
    }
}