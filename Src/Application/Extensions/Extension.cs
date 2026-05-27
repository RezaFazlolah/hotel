using Microsoft.EntityFrameworkCore;
using SharedKernel.Paging;

namespace Application.Extensions;

public static class Extension
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, PaginationParameters parameters)
    {
        var data = await source
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var pageNumber = parameters.PageNumber;
        var totalCount = await source.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

        var metaData = new PaginationMetadata
        {
            PageSize = parameters.PageSize,
            PageNumber = pageNumber,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };

        return new PagedResult<T> { Data = data, Metadata = metaData };
    }
}