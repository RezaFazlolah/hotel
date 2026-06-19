using Application.Hotels.Queries;
using Application.Interfaces.Repositories;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Handlers;

public class GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
    : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken ct)
        => await hotelRepository.GetByIdAsync(request.HotelId, ct);
}