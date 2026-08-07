using Application.Hotels.Commands;
using Application.Hotels.Dtos;
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
    public async Task<Result<HotelDto>> Handle(
        UpdateHotelCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update hotel {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        var updatedHotel = mapper.Map<Hotel>(request);

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var hotelIdResult = await managerRepository.GetHotelIdAsync(currentUserInfo.id, ct); 
            if (!hotelIdResult.Succeeded)
                return Result<HotelDto>.Failure(hotelIdResult.Errors.Prepend(rootError));
            var hotelId = hotelIdResult.Value;

            if (hotelId != request.Id)
                return Result<HotelDto>.Failure([rootError, new Error($"hotel not found", ErrorCode.Forbidden)],
                    ResultCode.Forbidden);
        }
        else
        {
            return Result<HotelDto>.Failure([rootError, new Error("forbidden request", ErrorCode.Forbidden)]);
        }

        var hotelUpdateResult = await hotelRepository.UpdateAsync(updatedHotel, ct);
        return hotelUpdateResult.Succeeded
            ? mapper.Map<Result<HotelDto>>(hotelUpdateResult)
            : Result<HotelDto>.Failure(hotelUpdateResult.Errors.Prepend(rootError));
    }
}