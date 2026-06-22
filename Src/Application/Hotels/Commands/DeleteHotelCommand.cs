using Api.Dtos.HotelDtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record DeleteHotelCommand(Guid HotelId)
    : IRequest<Result<HotelDto>>;