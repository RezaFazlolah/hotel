using Application.Interfaces.ServiceInterfaces;
using Application.Queries.HotelQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Handlers.HotelHandlers;

public class GetAllHotelsHandler(IHotelService hotelService)
    : IRequestHandler<GetAllHotels, Result<PagedResult<Hotel>>>
{
    public async Task<Result<PagedResult<Hotel>>> Handle(GetAllHotels request, CancellationToken ct)
        => await hotelService.GetAllAsync(
            request.PaginationParameters, ct);
}