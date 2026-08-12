using Application.Hotels.Dtos;
using Application.Hotels.Filters;
using Application.Hotels.Sorts;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Hotels.Queries;

public record GetAllHotelsQuery
    : IRequest<Result<PagedResult<HotelDto>>>
{
    public HotelFilterParameters? HotelFilterParameters { get; init; }
    public required HotelSortParameters HotelSortParameters { get; init; }
    public required PaginationParameters PaginationParameters { get; init; }
}