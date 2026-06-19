using MediatR;
using SharedKernel.Common;

namespace Application.Reservations.Commands;

public record CancelReservationCommand(Guid ReservationId)
    : IRequest<Result<Domain.Models.Reservation>>;