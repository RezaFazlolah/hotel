using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class DeleteHotelCommandHandler(
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<DeleteHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        DeleteHotelCommand request,
        CancellationToken ct)
    {
        var rolesResult = currentUserService.Roles;
        if (!rolesResult.Succeeded)
            return Result<HotelDto>.Failure(rolesResult.Errors);
        var roles = rolesResult.Value;

        // if (roles.Contains(UserRole.Admin))
        if (roles.Contains(UserRole.Admin))
        {
            var result = await hotelRepository.DeleteAsync(request.HotelId, ct);
            return mapper.Map<Result<HotelDto>>(result);
        }

        if (roles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.Id.Value;
            var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct); 
            if (!hotelIdResult.Succeeded)
                return Result<HotelDto>.Failure(
                    hotelIdResult.Errors.Prepend(
                        new Error($"delete hotel {request.HotelId} failed"))); // use InnerError instead
            var hotelId = hotelIdResult.Value;

            if (hotelId != request.HotelId)
                return Result<HotelDto>.Failure(
                    new Error($"delete hotel {request.HotelId} failed.hotel {request.HotelId} not found"));
            var result = await hotelRepository.DeleteAsync(request.HotelId, ct);
            return mapper.Map<Result<HotelDto>>(result);
        }

        return Result<HotelDto>.Failure(
            new Error($"delete hotel {request.HotelId} failed. forbidden request", ErrorCode.Forbidden),
            ResultCode.Forbidden);
    }
}