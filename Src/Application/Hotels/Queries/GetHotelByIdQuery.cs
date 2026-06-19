using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Queries;

public record GetHotelByIdQuery(Guid HotelId)
    : IRequest<Result<Domain.Models.Hotel>>;