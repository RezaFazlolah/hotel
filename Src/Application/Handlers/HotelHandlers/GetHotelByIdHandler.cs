using Application.Interfaces.ServiceInterfaces;
using Application.Queries.HotelQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.HotelHandlers;

public class GetHotelByIdHandler(IHotelService hotelService)
    : IRequestHandler<GetHotelById, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelById request, CancellationToken ct)
        => await hotelService.GetByIdAsync(request.HotelId, ct);
}