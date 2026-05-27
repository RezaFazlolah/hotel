namespace SharedKernel.Paging;

public class PaginationParameters
{
    // Todo: use fluent validation for PageNumber in range(1, int.Max) and PageSize in range(1, MaxPageSize)
    private int _pageSize = 10;
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize; 
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}