using Application.Hotels.Queries;
using Application.Interfaces.Repositories;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Hotels.Handlers;

public class GetAllHotelsQueryHandler(IHotelRepository hotelRepository)
    : IRequestHandler<GetAllHotelsQuery, Result<PagedResult<Hotel>>>
{
    public async Task<Result<PagedResult<Hotel>>> Handle(GetAllHotelsQuery request, CancellationToken ct)
        => await hotelRepository.GetAllAsync(
            request.PaginationParameters, ct);
}