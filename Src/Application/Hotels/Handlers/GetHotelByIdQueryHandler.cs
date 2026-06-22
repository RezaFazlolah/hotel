using Api.Dtos.HotelDtos;
using Application.Hotels.Queries;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Handlers;

public class GetHotelByIdQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken ct)
    {
        var result = await hotelRepository.GetByIdAsync(request.HotelId, ct);
        return mapper.Map<Result<HotelDto>>(result);
    }
}