namespace SharedKernel.Paging;

public class PaginationParameters
{
    // Question: is it a good idea to use fluent validation for PageNumber in range(1, int.Max) and PageSize in range(1, MaxPageSize)
    private int _pageSize = 10;
    private const int MaxPageSize = 50;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public int PageNumber { get; set; } = 1;
}