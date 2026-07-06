using SharedKernel.Sorts;

namespace Application.Extensions;

public static class SortExtension
{
    public static IQueryable SortAsync(
        this IQueryable query,
        BaseSortParameters sortParameters,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}