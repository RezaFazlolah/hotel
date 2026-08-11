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

public class InsertHotelCommandHandler(
    IHotelRepository hotelRepository,
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<InsertHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        InsertHotelCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"insert hotel failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<HotelDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (!currentUserInfo.roles.Contains(UserRole.Admin))
            return Result<HotelDto>.Forbidden(rootError);

        var hotel = mapper.Map<Hotel>(request);

        if (request.ManagerId.HasValue)
        {
            var managerId = request.ManagerId.Value;
            var managerResult = await managerRepository.GetByIdAsync(managerId, ct);
            if (!managerResult.Succeeded)
                return Result<HotelDto>.Failure(
                    [rootError, new Error($"manager {managerId} not found", ErrorCode.NotFound)], ResultCode.NotFound);
            var manager = (Manager)managerResult.Value;

            hotel.Manager = manager;
        }

        var result = await hotelRepository.InsertAsync(hotel, ct);
        return result.Succeeded
            ? mapper.Map<Result<HotelDto>>(result)
            : Result<HotelDto>.Failure(result.Errors.Prepend(rootError));
    }
}