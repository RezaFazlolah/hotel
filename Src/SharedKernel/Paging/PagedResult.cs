namespace SharedKernel.Paging;

public class PagedResult<T>
{
    public IReadOnlyList<T> Data { get; set; } = [];
    public PaginationMetadata Metadata { get; set; } = new();
}