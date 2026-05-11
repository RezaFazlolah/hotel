using Application.Interfaces.ServiceInterfaces;
using Application.Queries.HotelQueries;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetHotelByIdHandler(IHotelService hotelService)
    : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelService.GetByIdAsync(request.HotelId, cancellationToken);

        return hotel == null
            ? Result<Hotel>.Failure(new Error($"hotel {request.HotelId} not found"), code: 404)
            : Result<Hotel>.Success(hotel);
    }
}