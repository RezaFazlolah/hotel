using Application.Interfaces.Repositories;
using Application.Requests.HotelRequests;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.HotelHandlers;

public class UpdateHotelHandler(
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<UpdateHotel, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(UpdateHotel request, CancellationToken ct)
    {
        var currentUserRolesResult = await currentUserService.GetRolesAsync(ct);
        if (!currentUserRolesResult.Succeeded)
            return Result<Hotel>.Failure(
                currentUserRolesResult.Errors.Prepend(new Error($"update hotel {request.Id} failed.")));

        var currentUserRoles = currentUserRolesResult.Value;
        var updatedHotel = mapper.Map<Hotel>(request);

        if (currentUserRoles.Contains(UserRole.Admin))
            return await hotelRepository.UpdateAsync(updatedHotel, ct);
        if (currentUserRoles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.Id.Value;
            var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
            if (!hotelIdResult.Succeeded)
                return Result<Hotel>.Failure(hotelIdResult.Errors);
            var hotelId = hotelIdResult.Value;
            if (hotelId != request.Id)
                return Result<Hotel>.Failure(
                    new Error(
                        $"update hotel {request.Id} failed. manager {managerId} is not hotel {hotelId}'s manager.",
                        ErrorCode.Forbidden), ResultCode.Forbidden);
            return await hotelRepository.UpdateAsync(updatedHotel, ct);
        }

        return Result<Hotel>.Failure(
            new Error($"update hotel {request.Id} failed. unauthorized access", ErrorCode.Forbidden),
            ResultCode.Forbidden);
    }
}