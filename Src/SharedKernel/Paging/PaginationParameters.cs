namespace SharedKernel.Paging;

public class PaginationParameters
{
    // Question: is it a good idea to use fluent validation for PageNumber in range(1, int.Max) and PageSize in range(1, MaxPageSize)
    private int _pageNumber = 1;
    private int _pageSize = MaxPageSize;
    private const int MaxPageSize = 50;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > 0
            ? value
            : 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize
            ? MaxPageSize
            : value > 0
                ? value
                : 1;
    }
}