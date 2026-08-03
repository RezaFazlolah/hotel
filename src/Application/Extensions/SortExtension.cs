using SharedKernel.Sorts;

namespace Application.Extensions;

public static class SortExtension
{
    extension(IQueryable query)
    {
        public IQueryable SortAsync(
            BaseSortParameters sortParameters,
            CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}