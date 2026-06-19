using MediatR;
using SharedKernel.Common;
using SharedKernel.Paging;

namespace Application.Hotels.Queries;

public record GetAllHotelsQuery(PaginationParameters PaginationParameters)
    : IRequest<Result<PagedResult<Domain.Models.Hotel>>>;