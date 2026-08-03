namespace SharedKernel.Paginations;

public class PaginationMetadata
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }

    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    public bool HasNext => PageNumber < TotalPages;
    public bool HasPrevious => PageNumber > 1;
}