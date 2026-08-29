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

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            var managesHotel = await managerRepository.ManagesHotelAsync(currentUserInfo.id, request.Id, ct);
            if (!managesHotel)
                return Result<HotelDto>.Failure([rootError, new Error($"hotel not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }
        else
        {
            return Result<HotelDto>.Forbidden(rootError);
        }

        var updatedHotel = mapper.Map<Hotel>(request);
        var hotelUpdateResult = await hotelRepository.UpdateAsync(updatedHotel, ct);
        var hotelUpdateResultDto = mapper.Map<Result<HotelDto>>(hotelUpdateResult);
        return Result<HotelDto>.Handle(hotelUpdateResultDto, rootError);
    }
}