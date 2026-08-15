using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Rooms.Handlers;

public class InsertRoomCommandHandler(
    IRoomRepository roomRepository,
    IHotelRepository hotelRepository,
    IManagerRepository managerRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<InsertRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        InsertRoomCommand request,
        CancellationToken ct)
    {
        var rootError = new Error($"insert room failed");

        var currentUserInfoResult = currentUserService.Info;
        if (!currentUserInfoResult.Succeeded)
            return Result<RoomDto>.Failure(currentUserInfoResult.Errors.Prepend(rootError));
        var currentUserInfo = currentUserInfoResult.Value;

        if (currentUserInfo.roles.Contains(UserRole.Admin))
        {
            var hotelExists = await hotelRepository.ExistsAsync(request.HotelId, ct);
            if (!hotelExists)
                return Result<RoomDto>.Failure(
                    [rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesHotel(currentUserInfo.id, request.HotelId, ct))
                return Result<RoomDto>.Failure(
                    [rootError, new Error($"hotel {request.HotelId} not found", ErrorCode.NotFound)],
                    ResultCode.NotFound);
        }
        else
        {
            return Result<RoomDto>.Forbidden(rootError);
        }

        if (await roomRepository.NumberExistsAsync(request.HotelId, request.Number, ct))
            return Result<RoomDto>.Failure([
                rootError, new Error($"hotel {request.HotelId} already has room number {request.Number}")
            ]);

        var result = await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return Result<RoomDto>.Handle(resultDto, rootError);
    }
}