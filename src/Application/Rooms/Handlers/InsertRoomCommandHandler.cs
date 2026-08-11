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
            // if (!await hotelRepository.ExistsAsync(request.HotelId, ct))
            //     return Result<RoomDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found")]);
        }
        else if (currentUserInfo.roles.Contains(UserRole.Manager))
        {
            if (!await managerRepository.ManagesHotel(currentUserInfo.id, request.HotelId, ct))
                return Result<RoomDto>.Failure([rootError, new Error($"hotel {request.HotelId} not found")]);
        }
        else
            return Result<RoomDto>.Forbidden(rootError);

        if (await roomRepository.NumberExistsAsync(request.HotelId, request.Number, ct))
            return Result<RoomDto>.Failure([
                rootError, new Error($"hotel {request.HotelId} already has room number {request.Number}")
            ]);

        var roomInsertResult = await roomRepository.InsertAsync(mapper.Map<Room>(request), ct);
        return roomInsertResult.Succeeded
            ? mapper.Map<Result<RoomDto>>(roomInsertResult)
            : Result<RoomDto>.Failure(roomInsertResult.Errors.Prepend(rootError));
    }
}