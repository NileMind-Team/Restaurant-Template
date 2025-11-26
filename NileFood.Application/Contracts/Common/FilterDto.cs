namespace NileFood.Application.Contracts.Common;
public class FilterDto
{
    public string? PropertyName { get; set; }
    public string? PropertyValue { get; set; }
    public bool Range { get; set; }


    public static List<FilterDto> BindFromDictionary(Dictionary<string, string> filterDtos)
    {
        return filterDtos
            .Where(x => x.Key != nameof(UserParams.PageNumber) && x.Key != nameof(UserParams.PageSize))
            .Select(x => new FilterDto
            {
                PropertyName = x.Key,
                PropertyValue = x.Value,
            })
            .ToList();
    }
}
