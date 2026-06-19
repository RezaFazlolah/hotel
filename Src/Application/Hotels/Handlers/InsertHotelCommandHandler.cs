using Application.Hotels.Commands;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Handlers;

public class InsertHotelCommandHandler(
    IHotelRepository hotelRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<InsertHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(InsertHotelCommand request, CancellationToken ct)
    {
        // QUESTION: only admin can insert hotel, user role is checked at endpoint with [Authorize(Roles=UserRoleName.Admin)], do i need to check it here too?

        var hotel = mapper.Map<Hotel>(request);
        return await hotelRepository.InsertAsync(hotel, ct);
    }
}