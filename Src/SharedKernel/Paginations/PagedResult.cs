namespace SharedKernel.Paginations;

public class PagedResult<T>
{
    public required IReadOnlyList<T> Data { get; init; }
    public required PaginationMetadata Metadata { get; init; }
}