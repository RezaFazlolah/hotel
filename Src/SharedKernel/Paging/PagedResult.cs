namespace SharedKernel.Paging;

public class PagedResult<T>
{
    public IReadOnlyList<T> Data { get; init; } = [];
    public PaginationMetadata Metadata { get; init; } = new();
}