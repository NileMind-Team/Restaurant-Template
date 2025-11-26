namespace NileFood.Application.Contracts.Common;
public class UserParams
{
    private const int MaxPageSize = 10000000;
    public int? PageNumber { get; set; }
    private int? _pageSize = null;

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
