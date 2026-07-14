using SharedKernel.Paginations;

namespace SharedKernel.Interfaces;

public interface IPaginator
{
    PagedResult<T> Paginate<T>(
        IEnumerable<T> source,
        PaginationParameters paginationParameters,
        int totalCount);
}