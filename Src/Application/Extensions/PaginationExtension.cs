using Microsoft.EntityFrameworkCore;
using SharedKernel.Paging;

namespace Application.Extensions;

public static class PaginationExtension
{
    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        query = query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);

        return await CreatePagedResultAsync(query, paginationParameters, ct);
    }

    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IEnumerable<T> source,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var data = source
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .AsQueryable();

        return await CreatePagedResultAsync(data, paginationParameters, ct);
    }

    private static async Task<PagedResult<T>> CreatePagedResultAsync<T>(
        IQueryable<T> query,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var data = await query.ToListAsync(ct);
        
        var pageNumber = paginationParameters.PageNumber;
        var totalCount = data.Count;
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