using Application.Hotels.Commands;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class InsertHotelCommandHandler(
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<InsertHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(InsertHotelCommand request, CancellationToken ct)
    {
        var currentUserRolesResult = currentUserService.Roles;
        if(!currentUserRolesResult.Succeeded)
            return Result<Hotel>.Failure(currentUserRolesResult.Errors);
        var currentUserRoles = currentUserRolesResult.Value;
        
        if(!currentUserRoles.Contains(UserRole.Admin))
            return Result<Hotel>.Failure(new Error("insert hotel failed. forbidden access.", ErrorCode.Forbidden), ResultCode.Forbidden);
        
        var hotel = mapper.Map<Hotel>(request);
        return await hotelRepository.InsertAsync(hotel, ct);
    }
}