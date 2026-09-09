using Application.Common.Paginations;
using SharedKernel.Paginations;

namespace Infrastructure.Paginations;

public interface IPaginator
{
    PagedResult<T> Paginate<T>(
        IEnumerable<T> source,
        PaginationParameters paginationParameters,
        int totalCount);
}