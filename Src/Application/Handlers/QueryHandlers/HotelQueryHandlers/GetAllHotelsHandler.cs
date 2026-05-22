using Application.Interfaces.ServiceInterfaces;
using Application.Queries.HotelQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetAllHotelsHandler(IHotelService hotelService)
    : IRequestHandler<GetAllHotelsQuery, Result<ICollection<Hotel>>>
{
    public async Task<Result<ICollection<Hotel>>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        =>  await hotelService.GetAllAsync(
            cancellationToken,
            filterOn: request.FilterOn,
            filterQuery: request.FilterQuery,
            orderBy: request.OrderBy,
            isAscending: request.IsAscending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);
}