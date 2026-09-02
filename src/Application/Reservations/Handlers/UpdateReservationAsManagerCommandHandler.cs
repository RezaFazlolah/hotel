using Application.Interfaces.Services;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Handlers;

public class UpdateReservationAsManagerCommandHandler(
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateReservationAsManagerCommand, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(
        UpdateReservationAsManagerCommand request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}