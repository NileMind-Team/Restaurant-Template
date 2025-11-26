using NileFood.Application.Contracts.Common;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Helpers;
using NileFood.Infrastructure.Data;
using System.Linq.Expressions;
using System.Net;

namespace NileFood.Application.Services.Implementations;
public class FilterService<T> : IFilterService<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;
    public FilterService(ApplicationDbContext context)
    {
        _context = context;
        this.dbSet = _context.Set<T>();
    }
    public async Task<FilterResult<T>> Filter(
        List<FilterDto> filterDTOs,
        UserParams userParams,
        List<string>? includeProperties = null,
        Dictionary<string, List<string>>? thenIncludeProperties = null)
    {
        try
        {
            IQueryable<T> query = dbSet;
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "n");

            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                var deleteCheckParam = Expression.Parameter(typeof(T), "n");
                var prop = Expression.Property(deleteCheckParam, "IsDeleted");
                var condition = Expression.Equal(prop, Expression.Constant(false));
                var lambda = Expression.Lambda<Func<T, bool>>(condition, deleteCheckParam);
                query = query.Where(lambda);
            }

            foreach (var filterDto in filterDTOs)
            {
                if (string.IsNullOrEmpty(filterDto.PropertyName) || string.IsNullOrEmpty(filterDto.PropertyValue))
                    return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, "Please enter valid PropertyName and PropertyValue");

                filterDto.PropertyName = char.ToUpper(filterDto.PropertyName[0]) + filterDto.PropertyName.Substring(1);
                var propertyNames = filterDto.PropertyName.Split('.');
                Expression propertyExpression = parameter;

                foreach (var propertyName in propertyNames)
                    propertyExpression = Expression.Property(propertyExpression, propertyName);

                var propertyType = propertyExpression.Type;
                if (propertyType == null)
                    return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, $"Invalid PropertyName: {filterDto.PropertyName}");

                if (filterDto.PropertyValue.Contains("range"))
                {
                    filterDto.Range = true;
                    filterDto.PropertyValue = filterDto.PropertyValue.Replace(",range", "");
                }

                if (filterDto.Range && !string.IsNullOrEmpty(filterDto.PropertyValue))
                {
                    var rangeValues = filterDto.PropertyValue.Split(',');
                    if (rangeValues.Length != 2)
                        return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, "Range filter requires two values separated by a comma.");

                    if (!string.IsNullOrEmpty(rangeValues[0]))
                    {
                        var (success, value, error) = ConvertToType(rangeValues[0], propertyType);
                        if (!success) return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, error);

                        var startConstant = Expression.Constant(value, propertyType);
                        var startPredicate = Expression.GreaterThanOrEqual(propertyExpression, startConstant);
                        var startLambda = Expression.Lambda<Func<T, bool>>(startPredicate, parameter);
                        query = query.Where(startLambda);
                    }

                    if (!string.IsNullOrEmpty(rangeValues[1]))
                    {
                        var (success, value, error) = ConvertToType(rangeValues[1], propertyType);
                        if (!success) return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, error);

                        var endConstant = Expression.Constant(value, propertyType);
                        var endPredicate = Expression.LessThanOrEqual(propertyExpression, endConstant);
                        var endLambda = Expression.Lambda<Func<T, bool>>(endPredicate, parameter);
                        query = query.Where(endLambda);
                    }
                }
                else if (filterDto.PropertyValue.Split(',').Length > 1)
                {
                    var valuesAsStrings = filterDto.PropertyValue.Split(',');
                    var values = new List<object>();
                    foreach (var valStr in valuesAsStrings)
                    {
                        var (success, value, error) = ConvertToType(valStr, propertyType);
                        if (!success) return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, error);
                        values.Add(value);
                    }

                    Expression? combinedExpression = null;
                    foreach (var value in values)
                    {
                        var constant = Expression.Constant(value, propertyType);
                        var condition = Expression.Equal(propertyExpression, constant);
                        combinedExpression = combinedExpression == null ? condition : Expression.OrElse(combinedExpression, condition);
                    }

                    if (combinedExpression != null)
                    {
                        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                        query = query.Where(lambda);
                    }
                }
                else
                {
                    var (success, convertedValue, error) = ConvertToType(filterDto.PropertyValue, propertyType);
                    if (!success) return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.BadRequest, error);

                    var constant = Expression.Constant(convertedValue, propertyType);
                    var predicate = Expression.Equal(propertyExpression, constant);
                    var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                    query = query.Where(lambda);
                }
            }

            if (includeProperties != null)
            {
                foreach (var prop in includeProperties)
                {
                    foreach (var item in prop.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        query = query.Include(item);
                }
            }

            if (thenIncludeProperties != null)
            {
                foreach (var includeProperty in thenIncludeProperties)
                    query = ApplyThenInclude(query, includeProperty.Key, includeProperty.Value);
            }

            var totalCount = await query.CountAsync();
            if (totalCount == 0)
                return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.NotFound, "No matching data found.");

            var paginationRequests = query
                .Select(item => new PaginationRequest<T>
                {
                    Data = item,
                    Count = totalCount
                });

            var pagedData = await PagedList<PaginationRequest<T>>.CreateAsync(paginationRequests, userParams.PageNumber ?? 1, userParams.PageSize ?? 10);

            return new FilterResult<T>
            {
                Data = pagedData,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return Helper.CreateErrorResult<FilterResult<T>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private static IQueryable<T> ApplyThenInclude(IQueryable<T> query, string propertyName, List<string> thenIncludeProperties)
    {
        foreach (var thenIncludeProperty in thenIncludeProperties)
            query = query.Include($"{propertyName}.{thenIncludeProperty}");

        return query;
    }

    private static (bool Success, object? Value, string? ErrorMessage) ConvertToType(string value, Type targetType)
    {
        try
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType.IsEnum)
                return (true, System.Enum.Parse(underlyingType, value, true), null);

            if (underlyingType == typeof(DateOnly))
            {
                if (DateOnly.TryParse(value, out var dateOnlyValue))
                    return (true, dateOnlyValue, null);

                return (false, null, $"Invalid DateOnly format for value: '{value}'. Expected format YYYY-MM-DD.");
            }

            if (underlyingType == typeof(DateTime))
            {
                if (DateTime.TryParse(value, out var dateTimeValue))
                {
                    if (dateTimeValue.Kind == DateTimeKind.Unspecified)
                        dateTimeValue = DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc);
                    else if (dateTimeValue.Kind == DateTimeKind.Local)
                        dateTimeValue = dateTimeValue.ToUniversalTime();

                    return (true, dateTimeValue, null);
                }

                return (false, null, $"Invalid DateTime format for value: '{value}'.");
            }

            return (true, Convert.ChangeType(value, underlyingType), null);
        }
        catch (Exception ex)
        {
            return (false, null, $"Could not convert value '{value}' to type '{targetType.Name}'. Reason: {ex.Message}");
        }
    }
}



public class FilterResult<T> : OperationResult
{
    public PagedList<PaginationRequest<T>>? Data { get; set; }
}

public class PaginationRequest<T>
{
    public T? Data { get; set; }
    public int Count { get; set; }
}


public class PagedList<T> : List<T>
{
    public int? CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int? PageSize { get; set; }
    public int TotalCount { get; set; }

    public PagedList(IEnumerable<T> currentPage, int count, int? pageNumber, int? pageSize)
    {
        CurrentPage = pageNumber ?? 1;
        TotalPages = pageSize.HasValue ? (int)Math.Ceiling(count / (double)pageSize) : 1;
        PageSize = pageSize ?? count;
        TotalCount = count;
        AddRange(currentPage);
    }
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int? pageNumber, int? pageSize)
    {
        var count = await source.CountAsync();

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            source = source.Skip(pageNumber.Value * pageSize.Value).Take(pageSize.Value);
        }

        return new PagedList<T>(await source.ToListAsync(), count, pageNumber, pageSize);
    }
}