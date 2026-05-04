using Application.Models;
using Application.Queries.HotelQueries;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetHotelByIdHandler(IHotelService hotelService)
    : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelService.GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.HotelId} not found"), code: 404);
        return Result<Hotel>.Success(hotel);
    }
}