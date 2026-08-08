using Microsoft.EntityFrameworkCore;
using SharedKernel.Paginations;

namespace Infrastructure.Common;

public static class PaginationExtensions
{
    extension<T>(IQueryable<T> query)
    {
        public async Task<PagedResult<T>> PaginateAsync(
            PaginationParameters paginationParameters,
            CancellationToken ct)
        {
            var totalCount = await query.CountAsync(ct);

            var data = await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync(ct);

            return Paginate(data, paginationParameters, totalCount);
        }
    }

    // Smell: Paginate method is implemented twice, both here and Paginator.cs
    private static PagedResult<T> Paginate<T>(
        IEnumerable<T> source,
        PaginationParameters paginationParameters,
        int totalCount)
    {
        var metaData = new PaginationMetadata
        {
            PageSize = paginationParameters.PageSize,
            PageNumber = paginationParameters.PageNumber,
            TotalCount = totalCount
        };
    
        return new PagedResult<T>
        {
            Data = source.ToList(),
            Metadata = metaData
        };
    }
}