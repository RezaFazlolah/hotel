using Application.Common;
using Application.Hotels.Dtos;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Paginations;

namespace Application.Hotels.Queries;

public record GetAllHotelsQuery(PaginationParameters PaginationParameters)
    : IRequest<Result<PagedResult<HotelDto>>>;