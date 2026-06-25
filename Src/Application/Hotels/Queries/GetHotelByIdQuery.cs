using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Queries;

public record GetHotelByIdQuery(Guid HotelId)
    : IRequest<Result<HotelDto>>;