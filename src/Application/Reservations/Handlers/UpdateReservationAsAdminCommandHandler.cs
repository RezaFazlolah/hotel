using Application.Interfaces.Services;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Handlers;

public class UpdateReservationAsAdminCommandHandler(
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateReservationAsAdminCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        UpdateReservationAsAdminCommand request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}