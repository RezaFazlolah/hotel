using Application.Common.Paginations;
using SharedKernel.Paginations;

namespace Infrastructure.Paginations;

public class Paginator
    : IPaginator
{
    // Smell: Paginate method is implemented twice, both here and PaginationExtensions.cs
    public PagedResult<T> Paginate<T>(
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