using Application.Commands.HotelCommands;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.HotelHandlers;

public class UpdateHotelHandler(IHotelService hotelService, ICurrentUserService currentUserService, IManagerService managerService, IMapper mapper)
    : IRequestHandler<UpdateHotel, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(UpdateHotel request, CancellationToken ct)
    {
        var currentUserRolesResult = await currentUserService.GetRolesAsync(ct);
        if(!currentUserRolesResult.Succeeded)
            return Result<Hotel>.Failure(currentUserRolesResult.Errors);
        
        var currentUserRoles = currentUserRolesResult.Value;
        var updatedHotel = mapper.Map<Hotel>(request);

        if (currentUserRoles.Contains(UserRole.Admin))
            return await hotelService.UpdateAsync(updatedHotel, ct);
        if (currentUserRoles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.Id;
            var hotelIdResult = await managerService.GetHotelIdAsync(managerId, ct);
            if(!hotelIdResult.Succeeded)
                return Result<Hotel>.Failure(hotelIdResult.Errors);
            var hotelId = hotelIdResult.Value;
            if(hotelId!=request.Id)
                return Result<Hotel>.Failure(new Error($"update hotel failed. manager {managerId} is not hotel {hotelId}'s manager.", ErrorCode.Forbidden), ResultCode.Forbidden);
            return await hotelService.UpdateAsync(updatedHotel, ct);
        }
        return Result<Hotel>.Failure(
            new Error($"update hotel {request.Id} failed. unauthorized access", ErrorCode.Forbidden),
            ResultCode.Forbidden);
    }
}