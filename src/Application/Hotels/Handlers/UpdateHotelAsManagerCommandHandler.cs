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
        var rootError = new Error($"update hotel {request.Id} failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Manager))
            return Result<HotelDto>.Forbidden(rootError);

        var managesHotel = await managerRepository.ManagesHotelAsync(currentUserInfo.id, request.Id, ct);
        if (!managesHotel)
            return Result<HotelDto>.Failure([rootError, new Error($"hotel not found", ErrorCode.NotFound)],
                ResultCode.NotFound);

        var ratingResult = await hotelRepository.GetRatingAsync(request.Id, ct);
        if (!ratingResult.Succeeded)
            return Result<HotelDto>.Failure(ratingResult.Errors.Prepend(rootError));
        var rating = ratingResult.Value;

        var updatedHotel = mapper.Map<Hotel>(request);
        updatedHotel.Rating = rating;
        var updateResult = await hotelRepository.UpdateAsync(updatedHotel, ct);
        if (updateResult.Succeeded)
        {
            var managerResult = await managerRepository.GetByHotelIdAsync(request.Id, ct);
            if(managerResult.Succeeded)
                updateResult.Value.Manager=managerResult.Value;
        }
        var updateResultDto = updateResult.Map<Hotel, HotelDto>(mapper);
        return Result<HotelDto>.Handle(updateResultDto, rootError);
    }
}