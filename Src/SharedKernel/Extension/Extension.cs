using Microsoft.EntityFrameworkCore;
using SharedKernel.Paging;

namespace SharedKernel.Extension;

public static class Extension
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source,
        PaginationParameters paginationParameters, CancellationToken ct)
    {
        var data = await source
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(ct);

        var pageNumber = paginationParameters.PageNumber;
        var totalCount = await source.CountAsync(ct);
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationParameters.PageSize);

        var metaData = new PaginationMetadata
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = pageNumber,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };

        return new PagedResult<T> { Data = data, Metadata = metaData };
    }

    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source,
        PaginationParameters paginationParameters)
    {
        var data = source
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();

        var pageNumber = paginationParameters.PageNumber;
        var totalCount = source.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationParameters.PageSize);

        var metaData = new PaginationMetadata
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = pageNumber,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };

        return new PagedResult<T> { Data = data, Metadata = metaData };
    }
}