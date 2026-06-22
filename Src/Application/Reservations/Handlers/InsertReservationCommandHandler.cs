using Application.Dtos.ReservationDtos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Reservations.Commands;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Handlers;

public class InsertReservationCommandHandler(
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IMapper mapper)
    : IRequestHandler<InsertReservationCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(InsertReservationCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        // var errors = new List<Error>();
        // if (!await roomRepository.ExistsAsync(request.RoomId, cancellationToken))
        //     errors.Add(new Error($"room {request.RoomId} not found"));
        // if (await roomRepository.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate,
        //         cancellationToken))
        //     errors.Add(new Error($"room {request.RoomId} is reserved"));
        // if (errors.Count > 0)
        //     return Result<Reservation>.Failure(errors, 400);
        //
        // var currentUserId = currentUserService.Id;
        // var roles = await userRepository.GetRolesAsync(currentUserId, cancellationToken);
        //
        // if (roles.Contains(UserRole.Admin))
        //     // check errors
        //     // insert reservation
        //     _ = 3;
        // else if (roles.Contains(UserRole.Manager))
        //     // check errors
        //     // insert reservation            
        //     _ = 4; 
        // else if (roles.Contains(UserRole.Guest))
        //     // check errors
        //     // insert reservation            
        //     _ = 5; 
        // else
        //     return Result<Reservation>.Failure(new Error("user role not supported"), 403);
        //
        // var reservation = mapper.Map<Reservation>(request);
        // reservation.TotalPrice = await reservationRepository.CalculateTotalPriceAsync(request.RoomId, request.CheckInDate,
        //     request.CheckOutDate, cancellationToken);
        //
        // var result = await reservationRepository.InsertAsync(reservation, cancellationToken);
        // return result == null
        //     ? Result<Reservation>.Failure(new Error("reservation failed"), 400)
        //     : Result<Reservation>.Success(reservation, 201);
    }
}