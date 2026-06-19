using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record DeleteHotelCommand(Guid HotelId)
    : IRequest<Result<Domain.Models.Hotel>>;