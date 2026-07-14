using SharedKernel.Interfaces;

namespace SharedKernel.Paginations;

public class Paginator
    : IPaginator
{
    // Smell: Paginate method is implemented twice, SharedKernel/Paginations/Paginator.cs & SharedKernel/Paginations/PaginationExtensions.cs
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