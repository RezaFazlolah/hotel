using Application.Commands.HotelCommands;
using Application.Interfaces.ServiceInterfaces;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.HotelHandlers;

public class DeleteHotelHandler(IHotelService hotelService, ICurrentUserService currentUserService, IManagerService managerService)
    : IRequestHandler<DeleteHotel, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(DeleteHotel request, CancellationToken ct)
    {
        var rolesResult = await currentUserService.GetRolesAsync(ct);
        if(!rolesResult.Succeeded)
            return Result<Hotel>.Failure(rolesResult.Errors);
        var roles = rolesResult.Value;
        
        if(roles.Contains(UserRole.Admin))
            return await hotelService.DeleteAsync(request.HotelId, ct);
        if (roles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.Id;
            var hotelIdResult = await managerService.GetHotelIdAsync(managerId, ct);
            if(!hotelIdResult.Succeeded)
                return Result<Hotel>.Failure(hotelIdResult.Errors.Prepend(new Error($"delete hotel {request.HotelId} failed."))); // use InnerError instead
            var hotelId = hotelIdResult.Value;
            if(hotelId!=request.HotelId)
                return Result<Hotel>.Failure(new Error($"delete hotel {request.HotelId} failed.hotel {request.HotelId} not found."));
            return await hotelService.DeleteAsync(request.HotelId, ct);
        }
        return Result<Hotel>.Failure(new Error($"delete hotel {request.HotelId} failed. forbidden request", ErrorCode.Forbidden), ResultCode.Forbidden);
    }
}