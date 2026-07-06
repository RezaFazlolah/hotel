using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Paginations;

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var totalCount = query.Count();
        
        query = query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);

        return await CreatePagedResultAsync(query, paginationParameters, totalCount, ct);
    }

    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IEnumerable<T> source,
        PaginationParameters paginationParameters,
        CancellationToken ct)
    {
        var totalCount = source.Count();
        
        var query = source
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            // Question: is this implementation correct?
            .AsQueryable();

        return await CreatePagedResultAsync(query, paginationParameters, totalCount, ct);
    }

    private static async Task<PagedResult<T>> CreatePagedResultAsync<T>(
        IQueryable<T> query,
        PaginationParameters paginationParameters,
        int totalCount,
        CancellationToken ct)
    {
        var data = await query.ToListAsync(ct);

        var metaData = new PaginationMetadata
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = paginationParameters.PageNumber,
            TotalCount = totalCount
        };

        return new PagedResult<T>
        {
            Data = data,
            Metadata = metaData
        };
    }
}