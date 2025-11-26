using NileFood.Application.Contracts.Common;
using NileFood.Application.Services.Implementations;

namespace NileFood.Application.Services.Interfaces;
public interface IFilterService<T> where T : class
{
    Task<FilterResult<T>> Filter(List<FilterDto> filterDTOs, UserParams userParams, List<string>? includeProperties = null, Dictionary<string, List<string>>? thenIncludeProperties = null);
}
