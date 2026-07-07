using Application.Hotels.Dtos;
using Application.Hotels.Queries;
using Application.Interfaces.QueryServices;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Handlers;

public class GetHotelByIdQueryHandler(IHotelQueryService hotelQueryService)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken ct)
        => await hotelQueryService.GetByIdAsync(request.HotelId, ct);
}