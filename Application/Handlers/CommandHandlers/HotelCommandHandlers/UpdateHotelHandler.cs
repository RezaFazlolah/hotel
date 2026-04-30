using Application.Commands.HotelCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class UpdateHotelHandler(IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<UpdateHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (hotel == null)
            return Result<Hotel>.Failure(new Error($"hotel {request.Id} not found"), 404);

        mapper.Map(request, hotel);
        var updatedHotel = await hotelRepository.UpdateAsync(hotel, cancellationToken);
        if (updatedHotel == null)
            return Result<Hotel>.Failure(new Error($"update hotel {request.Id} failed"), 400);
        return Result<Hotel>.Success(mapper.Map<Hotel>(updatedHotel));
    }
}