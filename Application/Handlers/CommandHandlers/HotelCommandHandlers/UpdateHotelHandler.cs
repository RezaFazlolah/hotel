using Application.Commands.HotelCommands;
using Application.Interfaces.ServiceInterfaces;
using Application.Models;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class UpdateHotelHandler(IHotelService hotelService, IMapper mapper)
    : IRequestHandler<UpdateHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotelService.GetByIdAsync(request.Id, cancellationToken);
        if (hotel == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.Id} not found"), 404);

        mapper.Map(request, hotel);
        var updatedHotel = await hotelService.UpdateAsync(hotel, cancellationToken);

        return updatedHotel == null
            ? Result<Hotel>.Failure(new Error($"update hotel {request.Id} failed"), 400)
            : Result<Hotel>.Success(mapper.Map<Hotel>(updatedHotel));
    }
}