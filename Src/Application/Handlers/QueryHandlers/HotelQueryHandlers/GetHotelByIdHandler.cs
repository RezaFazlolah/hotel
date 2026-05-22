using Application.Interfaces.ServiceInterfaces;
using Application.Queries.HotelQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetHotelByIdHandler(IHotelService hotelService)
    : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken ct)
        => await hotelService.GetByIdAsync(request.HotelId, ct);
}