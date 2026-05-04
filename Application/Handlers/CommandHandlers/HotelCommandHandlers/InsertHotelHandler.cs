using Application.Commands.HotelCommands;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Domain.Services;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class InsertHotelHandler(IHotelService hotelService, IMapper mapper)
    : IRequestHandler<InsertHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(InsertHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = mapper.Map<Hotel>(request);
        var result = await hotelService.InsertAsync(hotel, cancellationToken);
        if (result == null)
            return Result<Hotel>.Failure(new Error("insert hotel failed"), 400);
        return Result<Hotel>.Success(result, 201);
    }
}