using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Filtering;
using SharedKernel.Paging;

namespace Application.Hotels.Queries;

public record GetAllHotelsQuery(
    HotelFilterParameters HotelFilterParameters,
    PaginationParameters PaginationParameters)
    : IRequest<Result<PagedResult<HotelDto>>>;