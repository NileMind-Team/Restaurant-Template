using System.Linq.Dynamic.Core;

namespace Egyptos.Application.Abstractions;

public class PaginatedList<T>
{
    public List<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        string? sortColumn = null,
        string? sortDirection = "ASC",
        string? searchValue = null,
        params string[] searchableColumns)
    {
        // ----------------------
        // Filtering (Search)
        // ----------------------
        if (!string.IsNullOrWhiteSpace(searchValue) && searchableColumns.Any())
        {
            // Support multiple columns for search
            var filterParts = searchableColumns.Select(c =>
                $"Convert({c}, \"System.String\").Contains(@0)"
            );

            var filter = string.Join(" OR ", filterParts);
            source = source.Where(filter, searchValue);
        }

        // ----------------------
        // Sorting
        // ----------------------
        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            var direction = sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";
            source = source.OrderBy($"{sortColumn} {direction}");
        }
        else
        {
            // Default sorting to avoid EF Core exception
            source = source.OrderBy("1"); // sort by first column
        }

        // ----------------------
        // Pagination
        // ----------------------
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}