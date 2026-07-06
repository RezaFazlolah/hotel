using Application.Hotels.Dtos;
using Domain.Models;
using MediatR;
using SharedKernel.Common;

namespace Application.Hotels.Commands;

public record InsertHotelCommand(
    string Name,
    string Address,
    float Rating)
    : IRequest<Result<HotelDto>>;