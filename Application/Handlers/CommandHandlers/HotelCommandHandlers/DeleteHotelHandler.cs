using Application.Commands.HotelCommands;
using Application.Models;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class DeleteHotelHandler(IHotelRepository hotelRepository)
    : IRequestHandler<DeleteHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        if (await hotelRepository.GetByIdAsync(request.HotelId, cancellationToken) == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.HotelId} not found"), 404);
        var result = await hotelRepository.DeleteAsync(request.HotelId, cancellationToken);
        if (result == null)
            return Result<Hotel>.Failure(new Error($"delete hotel {request.HotelId} failed"), 400);
        return Result<Hotel>.Success(result);
    }
}