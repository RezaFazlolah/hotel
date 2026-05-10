using Application.Commands.ReservationCommands;
using Application.Interfaces;
using Application.Interfaces.ServiceInterfaces;
using Application.Models;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Handlers.CommandHandlers.ReservationCommandHandlers;

public class InsertReservationHandler(
    IReservationService reservationService,
    IRoomService roomService,
    ICurrentUserService currentUserService,
    IUserService userService,
    IMapper mapper)
    : IRequestHandler<InsertReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(InsertReservationCommand request,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        if (!await roomService.ExistsAsync(request.RoomId, cancellationToken))
            errors.Add(new Error($"room {request.RoomId} not found"));
        if (await reservationService.IsReservedAsync(request.RoomId, request.CheckInDate, request.CheckOutDate,
                cancellationToken))
            errors.Add(new Error($"room {request.RoomId} is reserved"));
        if (errors.Count > 0)
            return Result<Reservation>.Failure(errors, 400);

        var currentUserId = currentUserService.CurrentUserId.Value;
        var roles = await userService.GetRolesAsync(currentUserId, cancellationToken);
        if (roles.Contains(UserRole.Guest))
            request.GuestId = currentUserId;
        else
            throw new NotImplementedException();

        var reservation = mapper.Map<Reservation>(request);
        reservation.TotalPrice = await reservationService.CalculateTotalPriceAsync(request.RoomId, request.CheckInDate,
            request.CheckOutDate, cancellationToken);

        var result = await reservationService.InsertAsync(reservation, cancellationToken);
        return result == null
            ? Result<Reservation>.Failure(new Error("reservation failed"), 400)
            : Result<Reservation>.Success(reservation, 201);
    }
}