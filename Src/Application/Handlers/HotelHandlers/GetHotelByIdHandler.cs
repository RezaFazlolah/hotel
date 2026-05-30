using Application.Interfaces.Repositories;
using Application.Requests.HotelRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.HotelHandlers;

public class GetHotelByIdHandler(IHotelRepository hotelRepository)
    : IRequestHandler<GetHotelById, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelById request, CancellationToken ct)
        => await hotelRepository.GetByIdAsync(request.HotelId, ct);
}