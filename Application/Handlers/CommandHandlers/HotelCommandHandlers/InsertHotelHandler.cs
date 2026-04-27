using Application.Commands.HotelCommands;
using Application.Result;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.CommandHandlers.HotelCommandHandlers;

public class InsertHotelHandler(IHotelRepository hotelRepository, IMapper mapper)
    : IRequestHandler<InsertHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(InsertHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = mapper.Map<Hotel>(request);
        var result = await hotelRepository.InsertAsync(hotel, cancellationToken);
        if (result == null)
            return Result<Hotel>.Failure("insert hotel failed", 400);
        return Result<Hotel>.Success(result);
    }
}