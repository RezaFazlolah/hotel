using Application.Commands.HotelCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Handlers.HotelHandlers;

public class InsertHotelHandler(IHotelService hotelService, IUserService userService, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<InsertHotel, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(InsertHotel request, CancellationToken ct)
    {
        // QUESTION: only admin can insert hotel, user role is checked at endpoint with [Authorize(Roles=UserRoleName.Admin)], do i need to check it here too?
        
        var hotel = mapper.Map<Hotel>(request);
        return await hotelService.InsertAsync(hotel, ct);
    }
}