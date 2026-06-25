using Application.Hotels.Commands;
using Application.Hotels.Dtos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Hotels.Handlers;

public class UpdateHotelCommandHandler(
    IHotelRepository hotelRepository,
    ICurrentUserService currentUserService,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<UpdateHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(UpdateHotelCommand request, CancellationToken ct)
    {
        var currentUserRolesResult = currentUserService.Roles;
        if (!currentUserRolesResult.Succeeded)
            return Result<HotelDto>.Failure(
                currentUserRolesResult.Errors.Prepend(new Error($"update hotel {request.Id} failed.")));
        var currentUserRoles = currentUserRolesResult.Value;
        
        var updatedHotel = mapper.Map<Hotel>(request);

        if (currentUserRoles.Contains(UserRole.Admin))
        {
            var hotelUpdateResult = await hotelRepository.UpdateAsync(updatedHotel, ct);
            return mapper.Map<Result<HotelDto>>(hotelUpdateResult);
        }

        if (currentUserRoles.Contains(UserRole.Manager))
        {
            var managerId = currentUserService.UserId.Value;
            var hotelIdResult = await managerRepository.GetHotelIdAsync(managerId, ct);
            if (!hotelIdResult.Succeeded)
                return Result<HotelDto>.Failure(hotelIdResult.Errors.Prepend(new Error($"update hotel {request.Id} failed.")));
            var hotelId = hotelIdResult.Value;
            
            if (hotelId != request.Id)
                return Result<HotelDto>.Failure(
                    new Error(
                        $"update hotel {request.Id} failed. hotel not found.",
                        ErrorCode.Forbidden), ResultCode.Forbidden);
            var hotelUpdateResult = await hotelRepository.UpdateAsync(updatedHotel, ct);
            return mapper.Map<Result<HotelDto>>(hotelUpdateResult);
        }

        return Result<HotelDto>.Failure(
            new Error($"update hotel {request.Id} failed. unauthorized access", ErrorCode.Forbidden),
            ResultCode.Forbidden);
    }
}