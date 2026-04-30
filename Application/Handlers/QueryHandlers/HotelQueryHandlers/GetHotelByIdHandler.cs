using Application.Models;
using Application.Queries.HotelQueries;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers.HotelQueryHandlers;

public class GetHotelByIdHandler(IHotelRepository hotelRepository)
    : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.HotelId} not found"), code: 404);
        return Result<Hotel>.Success(hotel);
    }
}