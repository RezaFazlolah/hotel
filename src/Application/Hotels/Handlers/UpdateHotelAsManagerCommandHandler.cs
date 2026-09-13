using Application.Common.Extensions;
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

public class UpdateHotelAsManagerCommandHandler(
    ICurrentUserService currentUserService,
    IHotelRepository hotelRepository,
    IManagerRepository managerRepository,
    IMapper mapper)
    : IRequestHandler<UpdateHotelAsManagerCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        UpdateHotelAsManagerCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"update hotel {request.HotelId} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Manager))
            return Result<HotelDto>.Forbidden(rootError);
        
        var managesHotel = await managerRepository.ManagesHotelAsync(currentUserInfo.id, request.HotelId, ct);
        if (!managesHotel)
            return Result<HotelDto>.Failure([rootError, new Error($"hotel not found", ErrorCode.NotFound)],
                ResultCode.NotFound);
        
        var hotelResult = await hotelRepository.GetByIdAsync(request.HotelId, ct);
        if(!hotelResult.Succeeded)
            return Result<HotelDto>.Failure(hotelResult.Errors.Prepend(rootError));
        var hotel = hotelResult.Value;

        mapper.Map(request, hotel);
        var updateResult = await hotelRepository.UpdateWithReloadAsync(hotel, ct);
        var updateResultDto = updateResult.Map<Hotel, HotelDto>(mapper);
        return Result<HotelDto>.Handle(updateResultDto, rootError);
    }
}