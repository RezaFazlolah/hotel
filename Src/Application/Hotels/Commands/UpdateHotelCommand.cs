using Api.Dtos.HotelDtos;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record UpdateHotelCommand(Guid Id, string Name, string Address, float Rating)
    : IRequest<Result<HotelDto>>;