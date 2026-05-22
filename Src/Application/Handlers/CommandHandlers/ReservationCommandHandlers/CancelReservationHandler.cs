using Application.Commands.ReservationCommands;
using Application.Interfaces;
using Application.Interfaces.ServiceInterfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Application.Handlers.CommandHandlers.ReservationCommandHandlers;

public class CancelReservationHandler(
    IReservationService reservationService,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<CancelReservationCommand, Result<Reservation>>
{
    public async Task<Result<Reservation>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        if (!await reservationService.ExistsAsync(request.ReservationId, cancellationToken))
            return Result<Reservation>.Failure(new Error($"reservation {request.ReservationId} not found"), ResultCode.NotFound);

        return await reservationService.CancelAsync(request.ReservationId, cancellationToken);
    }
}