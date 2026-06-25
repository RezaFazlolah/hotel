using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Hotels.Handlers;

public class GetAllHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<GetAllHotelsQuery, Result<PagedResult<HotelDto>>>
{
    public async Task<Result<PagedResult<HotelDto>>> Handle(GetAllHotelsQuery request, CancellationToken ct)
    {
        var result = await hotelRepository.GetAllAsync(
            request.PaginationParameters, ct);

        return mapper.Map<Result<PagedResult<HotelDto>>>(result);
    }
}