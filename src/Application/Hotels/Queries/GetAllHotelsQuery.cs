using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Hotels.Queries;

public record GetAllHotelsQuery
    : IRequest<Result<PagedResult<HotelDto>>>
{
    public required PaginationParameters PaginationParameters { get; init; }
}