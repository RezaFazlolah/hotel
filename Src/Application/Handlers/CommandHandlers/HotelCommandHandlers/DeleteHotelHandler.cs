using Application.Commands.HotelCommands;
using Application.Interfaces.ServiceInterfaces;
using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class DeleteHotelHandler(IHotelService hotelService)
    : IRequestHandler<DeleteHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        if (await hotelService.GetByIdAsync(request.HotelId, cancellationToken) == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.HotelId} not found"), 404);
        var result = await hotelService.DeleteAsync(request.HotelId, cancellationToken);
        return result == null
            ? Result<Hotel>.Failure(new Error($"delete hotel {request.HotelId} failed"), 400)
            : Result<Hotel>.Success(result);
    }
}