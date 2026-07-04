using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Hotels.Handlers;

public class GetAllHotelsQueryHandler(IHotelQueryService hotelQueryService)
    : IRequestHandler<GetAllHotelsQuery, Result<PagedResult<HotelDto>>>
{
    public async Task<Result<PagedResult<HotelDto>>> Handle(
        GetAllHotelsQuery request,
        CancellationToken ct)
        => await hotelQueryService.GetAllAsync(request.PaginationParameters, ct);
}