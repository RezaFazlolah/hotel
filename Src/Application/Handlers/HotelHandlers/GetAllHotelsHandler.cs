using Application.Interfaces.Repositories;
using Application.Requests.HotelRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Handlers.HotelHandlers;

public class GetAllHotelsHandler(IHotelRepository hotelRepository)
    : IRequestHandler<GetAllHotels, Result<PagedResult<Hotel>>>
{
    public async Task<Result<PagedResult<Hotel>>> Handle(GetAllHotels request, CancellationToken ct)
        => await hotelRepository.GetAllAsync(
            request.PaginationParameters, ct);
}