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

public class CreateHotelCommandHandler(
    IHotelRepository hotelRepository,
    IRoomRepository roomRepository,
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<CreateHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        CreateHotelCommand request,
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

            if (manager.HotelId != null)
                return Result<HotelDto>.Failure(
                    [rootError, new Error($"manager {managerId} already manages another hotel")]);

            hotel.Manager = manager;
        }

        var roomIds = request.RoomIds.ToList();
        if (roomIds.Count > 0)
        {
            foreach (var roomId in roomIds)
            {
                var roomResult = await roomRepository.GetByIdAsync(roomId, ct);
                if (!roomResult.Succeeded)
                    return Result<HotelDto>.Failure(roomResult.Errors.Prepend(rootError), ResultCode.NotFound);
                var room = roomResult.Value;

                hotel.Rooms.Add(room);
            }
        }

        var result = await hotelRepository.AddAsync(hotel, ct);
        var resultDto = result.Map<Hotel, HotelDto>(mapper);
        return Result<HotelDto>.Handle(resultDto, rootError);
    }
}