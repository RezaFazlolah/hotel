using Application.Interfaces.Repositories;
using Application.Requests.HotelRequests;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.HotelHandlers;

public class DeleteHotelHandler(
    IHotelRepository hotelRepository,
    ICurrentUserRepository currentUserRepository,
    IManagerRepository managerRepository)
    : IRequestHandler<DeleteHotel, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(DeleteHotel request, CancellationToken ct)
    {
        var rolesResult = await currentUserRepository.GetRolesAsync(ct);
        if (!rolesResult.Succeeded)
            return Result<Hotel>.Failure(rolesResult.Errors);
        var roles = rolesResult.Value;

        if (roles.Contains(UserRole.Admin))
            return await hotelRepository.DeleteAsync(request.HotelId, ct);
        if (roles.Contains(UserRole.Manager))
        {
            var managerId = currentUserRepository.Id.Value;
            var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
            if (!hotelIdResult.Succeeded)
                return Result<Hotel>.Failure(
                    hotelIdResult.Errors.Prepend(
                        new Error($"delete hotel {request.HotelId} failed."))); // use InnerError instead
            var hotelId = hotelIdResult.Value;
            if (hotelId != request.HotelId)
                return Result<Hotel>.Failure(
                    new Error($"delete hotel {request.HotelId} failed.hotel {request.HotelId} not found."));
            return await hotelRepository.DeleteAsync(request.HotelId, ct);
        }

        return Result<Hotel>.Failure(
            new Error($"delete hotel {request.HotelId} failed. forbidden request", ErrorCode.Forbidden),
            ResultCode.Forbidden);
    }
}