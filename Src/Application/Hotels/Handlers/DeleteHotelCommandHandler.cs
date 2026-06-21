using Application.Hotels.Commands;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class DeleteHotelCommandHandler(
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository)
    : IRequestHandler<DeleteHotelCommand, Result<Hotel>>
{
    public async Task<Result<Hotel>> Handle(DeleteHotelCommand request, CancellationToken ct)
    {
        var rolesResult = currentUserService.Roles;
        if (!rolesResult.Succeeded)
            return Result<Hotel>.Failure(rolesResult.Errors);
        var roles = rolesResult.Value;

        if (roles.Contains(UserRole.Admin))
            return await hotelRepository.DeleteAsync(request.HotelId, ct);
        if (roles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.Id.Value;
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